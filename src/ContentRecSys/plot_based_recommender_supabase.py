#!/usr/bin/env python
# coding: utf-8

import pandas as pd 
import numpy as np 
from supabase import create_client, Client
import logging
from flask import Flask, jsonify, request
from flask_ngrok import run_with_ngrok
import json
from waitress import serve
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import linear_kernel
import os

# Set up logging
logging.basicConfig(level=logging.INFO, format='%(asctime)s %(levelname)s:%(message)s')

# Your Supabase project details
URL = os.getenv('REC_SYS_SUPABASE_HOST')  # Supabase project URL
KEY = os.getenv('REC_SYS_SUPABASE_API_KEY') # Supabase API key
supabase: Client = create_client(URL, KEY)

def convert_table_to_pandas_dataframe(supabase, table_name):
    logging.info(f"Retrieving data from table {table_name}")
    # Retrieve data from Supabase
    data = supabase.table(table_name).select("*").execute()
    
    # Convert to DataFrame
    df = pd.DataFrame(data.data)
    logging.info(f"Data retrieved successfully from table {table_name}")
    return df

books_df = convert_table_to_pandas_dataframe(supabase, "books")

pd.set_option('display.max_colwidth', 50)
pd.set_option('display.max_columns', None)

books_df['combined'] = books_df['description'] + ' ' + books_df['title'] + ' ' + books_df['author_name']

# Content-based recommender setup
logging.info("Setting up the TF-IDF vectorizer and calculating TF-IDF matrix")
tfidf = TfidfVectorizer(stop_words='english')
tfidf_matrix = tfidf.fit_transform(books_df['combined'])

feature_names = tfidf.get_feature_names_out()
logging.info(f"TF-IDF matrix shape: {tfidf_matrix.shape}")
tfidf_matrix.shape

# Cosine similarity function for comparing every two documents
logging.info("Calculating cosine similarity matrix")
cosine_sim = linear_kernel(tfidf_matrix, tfidf_matrix)

indices = pd.Series(books_df.index, index=books_df['title']).drop_duplicates()

def get_original_book_id(title):
    return books_df.loc[books_df['title'] == title, 'id'].values[0]

def get_top_five_recommendations(title, cosine_sim=cosine_sim):
    logging.info(f"Generating recommendations for book: {title}")
    idx = indices[title]
    sim_scores = list(enumerate(cosine_sim[idx]))
    sim_scores = sorted(sim_scores, key=lambda x: x[1], reverse=True)
    sim_scores = sim_scores[:11]
    book_indices = [i[0] for i in sim_scores]

    ids = []
    for title in books_df['title'].iloc[book_indices]:
        ids.append(get_original_book_id(title))
    ids.pop(0)  # remove the first item
    logging.info(f"Top recommendations: {ids}")
    return ids

app = Flask(__name__)
run_with_ngrok(app)

def get_title(id):
    return books_df[books_df['id'] == id]['title'].values[0]

@app.route('/predict/<int:id>', methods=['GET'])
def predict(id):
    logging.info(f"Predict request received for book ID: {id}")
    try:
        title = get_title(id)
        logging.info(f"Book title for ID {id}: {title}")
        prediction_result = [int(x) for x in get_top_five_recommendations(title)]
        logging.info(f"Prediction result for book ID {id}: {prediction_result}")
        return json.dumps(prediction_result)
    except Exception as e:
        logging.error(f"Error during prediction: {e}")
        return jsonify({"error": str(e)}), 500

@app.route('/predict_with_title/<int:id>', methods=['GET'])
def predict_with_title(id):
    logging.info(f"Predict with title request received for book ID: {id}")
    try:
        title = get_title(id)
        logging.info(f"Book title for ID {id}: {title}")
        prediction_result = [get_title(int(x)) for x in get_top_five_recommendations(title)]
        response = {
            'title': title,
            'recommendations': prediction_result
        }
        logging.info(f"Prediction result with titles for book ID {id}: {response}")
        return jsonify(response)
    except Exception as e:
        logging.error(f"Error during prediction with title: {e}")
        return jsonify({"error": str(e)}), 500

if __name__ == '__main__': 
    logging.info("Starting Flask application")
    serve(app, host="0.0.0.0", port=8080)
