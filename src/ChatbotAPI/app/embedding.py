from sentence_transformers import SentenceTransformer

# Initialize Sentence Transformer model for text embedding
embedding_model = SentenceTransformer("thenlper/gte-small", cache_folder="cache")


def get_embedding(text: str) -> list[float]:
    """
    Generate embedding for the given text using the Sentence Transformer model.

    Args:
        text (str): Input text to generate embedding for.

    Returns:
        list[float]: Embedding vector for the input text.
    """
    if not text.strip():
        print("Attempted to get embedding for empty text.")
        return []

    # Encode text to obtain embedding
    embedding = embedding_model.encode(text, show_progress_bar=True, batch_size=64)

    return embedding.tolist()
