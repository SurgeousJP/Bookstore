FROM python:3.8-slim 

# Set working directory
WORKDIR /app

# Copy your application code (scripts, notebooks)
COPY . .

RUN pip install -r requirements.txt

EXPOSE 8080

# Command to run your application (replace with your actual command)
CMD ["python", "plot_based_recommender_supabase.py"]  
