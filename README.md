## Short Introduction

The solution has three layers: **Api**, **Application**, and **Infrastructure**.  
Each layer has its own dependency configuration.

- The **Api layer** is a thin layer responsible only for receiving HTTP requests and mapping responses returned from the Application layer.
- The **Application layer** is an orchestration layer that manages the flow of the code.
- The **Infrastructure layer** is responsible for storing and managing data access.

At application startup, a `HostedService` is triggered to fetch breweries from the provided URL.  
This service loads data into the configured database, whether it's **InMemory** or **SQLite**.

---

## Features & Specifications

### ✅ In-Memory storage + Bonus: SQLite + EF Core

- The Infrastructure layer provides two options: **InMemory** or **SQLite**, powered by **EF Core**.
- An interface `IBreweryRepository` is defined in the Application layer.
- Two implementations exist in Infrastructure:
  - `SqliteBreweryRepository`
  - `InMemoryBreweryRepository`

### ✅ Endpoints
Two main endpoints:
- Search for breweries
- Search and sort breweries by criteria

### ✅ Caching

- Custom `ICache` abstraction is introduced.
- Implemented using .NET's built-in `MemoryCache`.

### ✅ Error Handling

- Global exception handling is implemented using `.UseExceptionHandler`.
- The project follows a functional `Result<T, E>` monad pattern:
  - Next operation executes only if the previous one succeeded.
  - No unhandled exceptions.
  - All controllers inherit from a custom base `Controller<T>` class with a `Response<T>` method to unify result handling.

---

## Bonus Features

### ✅ API Versioning

- Built-in .NET API versioning is configured.
- Two versions of `BreweryController` are available.
- Swagger allows switching between versions via a dropdown.

### ✅ Logging

- Used extensively via `ILogger<T>` across layers.

### ✅ SQLite + EF Core

- Easily switch between InMemory and SQLite at startup using configuration.

### ✅ Simple API Key Authentication

- API key authentication implemented using custom `ApiKeyMiddleware`.
- Swagger UI allows passing the token via header.

