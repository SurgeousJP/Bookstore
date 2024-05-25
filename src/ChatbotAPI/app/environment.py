import os
from dotenv import load_dotenv

# Load environment variables from .env file
load_dotenv()
# Access environment variables
HF_TOKEN = os.getenv("HF_TOKEN")
MONGO_URI = os.getenv("MONGO_URI")
GOOGLE_API_KEY = os.getenv("GOOGLE_API_KEY")