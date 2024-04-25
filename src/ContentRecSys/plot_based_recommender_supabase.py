#!/usr/bin/env python
# coding: utf-8

import pandas as pd 
import numpy as np 
from supabase import create_client, Client

# Your Supabase project details
URL = "https://oflclzbsbgkadqiagxqk.supabase.co"  # Supabase project URL
KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im9mbGNsemJzYmdrYWRxaWFneHFrIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MDY0OTY3OTIsImV4cCI6MjAyMjA3Mjc5Mn0.2IGuSFqHbNp75vs-LskGjK0fw3ypqbiHJ9MKAAaYE8s"                    # Supabase API key
supabase: Client = create_client(URL, KEY)

def convert_table_to_pandas_dataframe(supabase, table_name):
    # Retrieve data from Supabase
    data = supabase.table(table_name).select("*").execute()
        
    # Convert to DataFrame
    df = pd.DataFrame(data.data)

    return df

books_df = convert_table_to_pandas_dataframe(supabase, "books")

pd.set_option('display.max_colwidth', 50)
pd.set_option('display.max_columns', None)

books_df.head(5)

books_df['combined'] = books_df['description'] + ' ' + books_df['title'] + ' ' + books_df['author_name']


# Content-based recommender

# Trong khai phá dữ liệu văn bản (text mining), thuật ngữ TF-IDF (term frequency - inverse document frequency) là một phương thức thống kê được biết đến rộng rãi nhất để xác định độ quan trọng của một từ trong đoạn văn bản trong một tập nhiều đoạn văn bản khác nhau. 
# 
# TF (Term Frequency): là tần suất xuất hiện của một từ trong một đoạn văn bản.
# TF(t) = f(t,d)/T
# (t là từ, f(t,d) là tần suất xuất hiện từ t trong đoạn d, T là tổng số từ trong đoạn văn T)
# 
# IDF (Inverse Document Frequency): tính toán độ quan trọng của một từ. Khi tính toán TF, mỗi từ đều quan trọng như nhau, nhưng có một số từ trong tiếng Anh như "is", "of", "that",... xuất hiện khá nhiều nhưng lại rất ít quan trọng. Vì vậy, chúng ta cần một phương thức bù trừ những từ xuất hiện nhiều lần và tăng độ quan trọng của những từ ít xuất hiện những có ý nghĩa đặc biệt cho một số đoạn văn bản hơn bằng cách tính IDF.
# 
# IDF(t) = log(N/∣t∈D:t∈d∣) 
# (N là tổng số đoạn văn bản)
# 
# TF-IDF(t) = TF(t) * IDF(t)

#Import TfIdfVectorizer from scikit-learn
from sklearn.feature_extraction.text import TfidfVectorizer

#Define a TF-IDF Vectorizer Object. Remove all english stop words such as 'the', 'a'
tfidf = TfidfVectorizer(stop_words='english')

#Construct the required TF-IDF matrix by fitting and transforming the data
tfidf_matrix = tfidf.fit_transform(books_df['combined'])

feature_names = tfidf.get_feature_names() 

#Output the shape of tfidf_matrix
tfidf_matrix.shape


# 20183 -> total document in the corpus
# 141956 -> total distinct terms in the corpus 

feature_names[2000:2500]

# Assuming 'tfidf_matrix' is your TF-IDF matrix
# Assuming 'document_index' is the index of the document you want to calculate the total terms for

# Get the TF-IDF vector for the specified document
document_tfidf_vector = tfidf_matrix[10]

# Sum up the TF-IDF weights for all terms in the document
total_terms_in_document = document_tfidf_vector.sum()

print("Document vector: ", tfidf_matrix[10])
print("Total terms in document {}: {}".format(10, total_terms_in_document))

tfidf

print(tfidf_matrix[0].shape)

# Cosine similarity function for comparing every two documents
# Import linear_kernel
from sklearn.metrics.pairwise import linear_kernel

# Compute the cosine similarity matrix
cosine_sim = linear_kernel(tfidf_matrix, tfidf_matrix)

indices = pd.Series(books_df.index, index=books_df['title']).drop_duplicates()

def get_original_book_id(title):
    return books_df.loc[books_df['title'] == title, 'id'].values[0]

# Function that takes in movie title as input and outputs most similar movies
def get_top_five_recommendations(title, cosine_sim=cosine_sim):
    # Get the index of the movie that matches the title
    idx = indices[title]

    # Get the pairwsie similarity scores of all movies with that movie
    sim_scores = list(enumerate(cosine_sim[idx]))
    
    # Sort the movies based on the similarity scores
    sim_scores = sorted(sim_scores, key=lambda x: x[1], reverse=True)

    # Get the scores of the 10 most similar movies
    sim_scores = sim_scores[:11]

    # Get the movie indices
    book_indices = [i[0] for i in sim_scores]

#     # Return the top 10 most similar movies
#     return books_df['title'].iloc[book_indices]

    ids = []
    for title in books_df['title'].iloc[book_indices]:
        ids.append(get_original_book_id(title))
    ids.pop(0)
    return ids

get_top_five_recommendations('Walls of Ash')

books_df[books_df['id'].isin(get_top_five_recommendations('Walls of Ash'))]['url']


from flask import Flask, jsonify, request
from flask_ngrok import run_with_ngrok

app = Flask(__name__)
run_with_ngrok(app)  # Start ngrok when app is run

import json
@app.route('/predict/<int:id>', methods=['GET'])
def predict(id):
    title = books_df[books_df['id'] == id]['title'].values[0]
    print(title)
    prediction_result = [int(x) for x in get_top_five_recommendations(title)]
    return json.dumps(prediction_result)

from waitress import serve

if __name__ == '__main__': 
    serve(app, host="0.0.0.0", port=8080)

