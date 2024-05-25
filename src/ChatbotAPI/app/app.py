from flask import Flask, jsonify, request
from database import get_search_result
from ai import generate_response

# Initialize Flask app
app = Flask(__name__)


@app.route('/generate', methods=['POST'])
def chatbot():
    data = request.get_json()
    query = data.get('query', '')
    # If trimmed query is empty, return an error response
    if not query.strip():
        return jsonify({"response": "Invalid query. Please provide a valid query."})

    # Generate search results based on the user query
    search_result, ids = get_search_result(query)

    # Execute the LLMChain to generate response
    try:
        result = generate_response(query, search_result)
        return jsonify({"response": result,
                    "ids": ids,})
    except Exception as e:
        return jsonify({"response": "An error occurred while generating response. Please try again.", ids: []})


if __name__ == '__main__':
    app.run(port=8080, host="0.0.0.0", debug=True)
