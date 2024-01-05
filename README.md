# Carpool MSN

This project represents an ASP.NET Core API designed to provide carpooling functionalities. The API uses PostgreSQL as the database to store user information, trips, etc.

## Prerequisites

Before getting started, make sure you have the following tools installed:

- [.NET SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)

## Project Structure Overview
1. **Carpool.API :** Entry point, handling HTTP requests via controllers defining accessible functionalities.
2. **Carpool.Application :** The functional core, housing business logic through services orchestrating operations.
3. **Carpool.Domain :** The kernel, defining entities and value objects representing business concepts.
4. **Carpool.Infrastructure :** Handling data persistence with concrete implementations of the repositories.
5. **Carpool.Tests :** Containing unit tests ensuring the reliability of each component of the application.

## Setting up PostgreSQL
### Installing PostgreSQL

- If you haven't installed PostgreSQL on your machine yet, you can do so by downloading the appropriate installer from the [official PostgreSQL website](https://www.postgresql.org/download/).

### Configuring PostgreSQL

- Ensure that the PostgreSQL service is running on your machine.
- Use the command line or a graphical tool to connect to PostgreSQL using the superuser or a role with database creation rights.
```bash
  sudo -i -u postgres
```
```bash
  psql
```

### Creating a database for your application

1. Connect to PostgreSQL.
2. Create a database for your application:
```sql
  CREATE DATABASE YourDatabaseName;
```

### Creating a dedicated user for the database application
- For improved security, create a dedicated user for your application and grant appropriate privileges to the database you just created:
```sql
  CREATE USER YourUserName WITH PASSWORD 'YourPassword';
  GRANT ALL PRIVILEGES ON DATABASE YourDatabaseName TO YourUserName;
```

## API Setup
1. Clone this repository to your local machine.
2. Ensure that PostgreSQL is properly set up as per the earlier instructions.
3. Open the project in your preferred code editor.

### Configuration in your ASP.NET core application
Before running the API, make sure to set up the environment by creating an `appsettings.json` file at the root of the API folder with the following structure:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=YourDatabaseName;Username=YourUserName;Password=YourPassword;"
  },
  "Jwt": {
    "SecretKey": "HMAC-SHA256_Secret_Key",
    "Issuer": "Your_Issuer",
    "Audience": "Your_Audience",
  }
  // Other configurations here
}
```
- Make sure to replace YourDatabaseName, YourUser, and YourPassword with the connection details for your PostgreSQL database.
- Database services were added in Program.cs.

### Running the API
To start the API, execute the following command in the terminal at the root of the API folder:
```bash
  dotnet run
```
The API will be accessible at `https://localhost:5011` by default.

### Available Endpoints

#### UserController
- `GET /users`: Retrieve all registered users. (Administrator access only.)
  - Example: `GET https://localhost:5011/users`

- `GET /users/{id}`: Retrieve a user by ID. (Administrator access only.)
  - Example: `GET https://localhost:5011/users/123`

- `PUT /users/{id}`: Update a user by ID.
  - Example: `PUT https://localhost:5011/users/123`

- `DELETE /users/{id}`: Delete a user by ID.
  - Example: `DELETE https://localhost:5011/users/123`

#### AuthController

- `POST /auth/register`: Register a new user.
  - Example: `POST https://localhost:5011/auth/register`

- `POST /auth/login`: Log in a user.
  - Example: `POST https://localhost:5011/auth/login`

- `POST /auth/logout`: Log out a user.
  - Example: `POST https://localhost:5011/auth/logout`

- `POST /auth/refresh-token`: Refresh the authentication token.
  - Example: `POST https://localhost:5011/auth/refresh-token`

#### AddressController

- `GET /addresses`: Retrieve all addresses.
  - Example: `GET https://localhost:5011/addresses`

- `GET /addresses/{id}`: Retrieve an address by ID.
  - Example: `GET https://localhost:5011/addresses/123`

- `POST /addresses`: Create a new address.
  - Example: `POST https://localhost:5011/addresses`

- `PUT /addresses/{id}`: Update an address by ID.
  - Example: `PUT https://localhost:5011/addresses/123`

- `DELETE /addresses/{id}`: Delete an address by ID.
  - Example: `DELETE https://localhost:5011/addresses/123`

#### CarController

- `GET /cars`: Retrieve all cars (Administrator access only).
  - Example: `GET https://localhost:5011/cars`

- `GET /cars/user/{userId}`: Retrieve cars by user ID (Administrator access only).
  - Example: `GET https://localhost:5011/cars/user/123`

- `GET /cars/{id}`: Retrieve a car by ID (Administrator access only).
  - Example: `GET https://localhost:5011/cars/456`

- `POST /cars`: Add a new car.
  - Example: `POST https://localhost:5011/cars`

- `PUT /cars/{id}`: Update a car by ID.
  - Example: `PUT https://localhost:5011/cars/789`

- `DELETE /cars/{id}`: Remove a car by ID.
  - Example: `DELETE https://localhost:5011/cars/789`

#### FeedbackController

- `GET /feedbacks`: Retrieve all feedbacks (Administrator access only).
  - Example: `GET https://localhost:5011/feedbacks`

- `GET /feedbacks/{id}`: Retrieve feedback by ID.
  - Example: `GET https://localhost:5011/feedbacks/123`

- `GET /feedbacks/user/{userId}`: Retrieve feedbacks by user ID.
  - Example: `GET https://localhost:5011/feedbacks/user/456`

- `POST /feedbacks`: Add a new feedback.
  - Example: `POST https://localhost:5011/feedbacks`

- `PUT /feedbacks/{id}`: Update a feedback by ID.
  - Example: `PUT https://localhost:5011/feedbacks/789`

- `DELETE /feedbacks/{id}`: Delete a feedback by ID.
  - Example: `DELETE https://localhost:5011/feedbacks/789`

#### ReservationController

- `GET /reservations`: Retrieve all reservations (Administrator access only).
  - Example: `GET https://localhost:5011/reservations`

- `GET /reservations/user/{userId}`: Retrieve reservations by user ID.
  - Example: `GET https://localhost:5011/reservations/user/456`

- `GET /reservations/trip/{tripId}`: Retrieve reservations by trip ID.
  - Example: `GET https://localhost:5011/reservations/trip/789`

- `GET /reservations/{id}`: Retrieve reservation by ID.
  - Example: `GET https://localhost:5011/reservations/123`

- `POST /reservations`: Add a new reservation.
  - Example: `POST https://localhost:5011/reservations`

- `DELETE /reservations/cancel/{id}`: Cancel a reservation by ID.
  - Example: `DELETE https://localhost:5011/reservations/cancel/789`

TripController

- `GET /trips`: Retrieve all trips.
  - Example: `GET https://localhost:5011/trips`

- `GET /trips/{id}`: Retrieve a trip by ID.
  - Example: `GET https://localhost:5011/trips/123`

- `POST /trips`: Create a new trip.
  - Example: `POST https://localhost:5011/trips`

- `PUT /trips/{id}`: Update a trip by ID.
  - Example: `PUT https://localhost:5011/trips/123`

- `DELETE /trips/{id}`: Delete a trip by ID.
  - Example: `DELETE https://localhost:5011/trips/123`

### Middlewares
#### Exception Handling Middleware

The Exception Handling Middleware intercepts exceptions that occur during the processing of HTTP requests. It logs the details of these exceptions and returns a JSON response containing appropriate error information to the API clients. This middleware facilitates consistent error management within the application.

## Tests
### Running the tests
1. Create a `appsettings.test.json` at the root of Carpool.Tests.
2. Add the following structure in the file:
```json
{
  "Jwt": {
    "SecretKey": "HMAC-SHA256_Secret_Key",
    "Issuer": "Your_Issuer",
    "Audience": "Your_Audience"
  }
}
```
3. Open a command line at the root of the test folder.
4. Run the following command to execute all tests:
```bash
  dotnet test
```

### Test Structure
Tests are organized according to the following structure:

- **Carpool.Tests**
  - **Controllers**: Tests for API controllers.
  - **Services**: Tests for business logic services.
  - **Repositories**: Tests for data access repositories.
  - **Utilities**: Utility/helper classes or functions for tests.


## Contributions
Contributions are welcome! If you wish to contribute to this project, make sure to create a branch for your changes and submit a pull request when ready.

## Issues and Questions
If you encounter any issues or have questions, feel free to open a ticket in the "Issues" section of the repository.

## To do:
- Setting tokens expiry date dynamically in following files:
  - JwtService.cs
  - TokenManagerService.cs

- Creating a Dashboard repository, service and controller
- Fixing nullable properties in Entities
- Creating DTOs for each Services, controllers
- Check for more error handling in services

Thank you for using our API!