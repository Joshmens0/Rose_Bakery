# Rose Bakery API

Rose Bakery is a C# ASP.NET Core web application that provides an API for managing a bakery's operations, including products, categories, orders, collections, and users.

## Architecture

The project follows an N-tier architecture to separate concerns:
- **Controllers Layer:** Handles incoming HTTP requests and routes them to the appropriate services.
- **Service Layer:** Contains the business logic and orchestrates data retrieval and manipulation.
- **Data Layer:** Interacts with the PostgreSQL database using Entity Framework Core.
- **Models & DTOs:** Defines the structure of data stored in the database and transferred between the API and clients.

## Technologies Used

- **Framework:** ASP.NET Core (.NET 8.0)
- **Database:** PostgreSQL
- **ORM:** Entity Framework Core (`Npgsql.EntityFrameworkCore.PostgreSQL`)
- **Authentication:** JSON Web Tokens (JWT)
- **API Documentation:** Swagger / OpenAPI (`Swashbuckle.AspNetCore`)
- **Testing:** xUnit and Moq

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/) running locally or accessible remotely.

## Getting Started

1. **Clone the repository:**
   ```bash
   git clone <repository_url>
   cd Rose_Bakery
   ```

2. **Configure the Database Connection:**
   Update the `DefaultConnection` string in `Rose_Bakery/appsettings.json` to match your PostgreSQL instance configuration. For example:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "server=localhost;username=postgres;database=bakery;password=your_password;port=5432"
   }
   ```

3. **Apply Database Migrations:**
   Ensure your database is up to date with the latest EF Core migrations.
   ```bash
   cd Rose_Bakery
   dotnet ef database update
   ```

4. **Run the Application:**
   ```bash
   dotnet run
   ```
   The API will be available at `http://localhost:<port>` or `https://localhost:<port>`. In development mode, you can access the Swagger UI documentation at `/swagger/index.html`.

## API Endpoints

The API includes endpoints for the following areas:

- **Products (`/api/Product`):** Create, retrieve, update, and delete products.
- **Categories (`/api/Category`):** Manage product categories.
- **Orders (`/api/Order`):** Create and manage customer orders.
- **Bakery Collection (`/api/BakeryCollection`):** Retrieve aggregated bakery collections and paged views.
- **Users (`/api/User`):** User registration and authentication endpoints.

*Note: Some endpoints require authorization. You can authenticate via the `/api/User/Login` endpoint to obtain a JWT token, which should be included in the `Authorization` header (`Bearer <token>`).*

## Testing

The repository contains a test project `Rose_Bakery.Tests` that utilizes xUnit and Moq. It includes a custom `DbSetMock` helper for testing EF Core's asynchronous data operations.

To run the tests:
```bash
cd Rose_Bakery.Tests
dotnet test
```
