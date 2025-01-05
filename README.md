# Bookstore Backend Microservice

# Note
This project require an env store in the project src file to work like this:
CLOUDINARY_URL=
CLOUDINARY_CLOUDNAME=
CLOUDINARY_APIKEY=
CLOUDINARY_SECRET=
JWT_SECRET=

EMAIL_SENDER=
SMTP_PASSWORD=

CATALOG_DB_CONNECTION_STRING=
IDENTITY_DB_CONNECTION_STRING=
BASKET_CONNECTION_STRING=
BASKET_CONNECTION_PASSWORD=
ORDER_CONNECTION_STRING=
REC_SYS_SUPABASE_HOST=
REC_SYS_SUPABASE_API_KEY=

STRIPE_DEVICE_NAME=
STRIPE_API_KEY=
WEBHOOK_SECRET_KEY=


![Swagger Backend](https://github.com/user-attachments/assets/a7d115b1-e49c-4c45-b325-1ce645f2b5ba)

### To view the repo containing the frontend of this project, please follow this [link](https://github.com/4nh3k/BookStoreFE).

A Bookstore E-Commerce backend server built locally based on Microservice Architecture using .NET & Docker.

## Features
- A Catalog API for CRUD operations on book products.
- An Identity API & JWT authentication for Authentication & Authorization.
- An Order API server for handling order requests
- A Basket API built by Redis & .NET for caching user's cart.
- A simple recommendation API suggesting books which are similar to the book selected.

## Installation
1. Clone the repository
 `git clone https://github.com/4nh3k/BookStore.git`
2. Open the terminal in project root and change directory to src folder using: cd src
3. Create .env file which has the structured like specified in the note above in the src folder and fill in your own services
4. Build the project using the command: docker-compose build
5. After the build is completed, run the containers by running: docker-compose up -d
6. Open the backend server gateway using the url: https://localhost:6002/swagger/index.html (don't use the url contain port 6001 since it is http)
7. For the Order & Basket API to run, please login through the login api in the Identity API and assign the JWT token to the bearer in these APIs (account: admin admin)
