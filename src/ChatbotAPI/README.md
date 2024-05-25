# Chat Bot

This is a chat bot application that utilizes various APIs and services.

## Prerequisites

Before running the application, make sure you have the following:

- Docker installed on your machine
- An `.env` file with the following environment variables:
  - `HF_TOKEN`: Hugging Face Hub API token
  - `GOOGLE_API_KEY`: Google API key
  - `MONGO_URI`: MongoDB connection URI

## Build and Run

To build and run the chat bot application, follow these steps:

1. Open a terminal and navigate to the project directory.

2. Build the Docker image using the following command:

   ```shell
   docker build -t chat_bot .
   ```

3. Run the Docker container in detached mode and map port 5000 to the host using the following command:

   ```shell
   docker run -d -p 5000:5000 chat_bot
   ```

4. The chat bot application should now be running. You can access it by opening a web browser and navigating to `http://localhost:5000`.

## License

This project is licensed under the [MIT License](LICENSE).
