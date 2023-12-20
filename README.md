# Carpool MSN

This project represents an ASP.NET Core API designed to provide carpooling functionalities. The API uses PostgreSQL as the database to store user information, trips, etc.

## Prerequisites

Before getting started, make sure you have the following tools installed:

- [.NET SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)

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
Before running the API, make sure to set up the environment by creating an `appsettings.json` file at the root of the project with the following structure:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=YourDatabaseName;Username=YourUserName;Password=YourPassword;"
  },
  // Other configurations here
}
```
- Make sure to replace YourDatabaseName, YourUser, and YourPassword with the connection details for your PostgreSQL database.
- Database services were added in Program.cs.

### Running the API
To start the API, execute the following command in the terminal at the root of the project:
```bash
dotnet run
```
The API will be accessible at https://localhost:5011 by default.

### Available Endpoints
- `/users`: Get all registered users.

### Middlewares
#### Exception Handling Middleware

The Exception Handling Middleware intercepts exceptions that occur during the processing of HTTP requests. It logs the details of these exceptions and returns a JSON response containing appropriate error information to the API clients. This middleware facilitates consistent error management within the application.


## Contributions
Contributions are welcome! If you wish to contribute to this project, make sure to create a branch for your changes and submit a pull request when ready.

## Issues and Questions
If you encounter any issues or have questions, feel free to open a ticket in the "Issues" section of the repository.

Thank you for using our API!