# BreweryAPI
ASP.NET local API, connection by local IP address.

Have some features as:

- CRUD requests; User and Admin generates salt via POST request to encrypt the password.
- GET by ID requests;
- Logical delete and restore requests (to use with more than 1 position);
- Search with pagination;
- PUT request to change the user/admin password with encryption and salt using;
- Getting and using auth key to authorize via registry values;
