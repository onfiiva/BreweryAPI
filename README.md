# BreweryAPI
ASP.NET local API, connection by local IP address.

Using Brewery.sql MS SQL Database.

DB have some features as:

- Columns for logical delete;
- Token table to use tokens;
- User and Admin have salt fields;

API have some features as:

- CRUD requests; User and Admin generates salt via POST request to encrypt the password.
- GET by ID requests;
- Logical delete and restore requests (to use with more than 1 position);
- Search with pagination;
- PUT request to change the user/admin password with encryption and salt using;
- Getting and using auth key to authorize via registry values;
