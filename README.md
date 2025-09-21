
## Description

Provide a brief overview of your application. What problem does it solve? What are its key features?
For example: "This ASP.NET Core application helps users plan and book tours. It allows browsing destinations, creating custom itineraries, and managing bookings."

## Tech Stack

List the technologies used in your project. Include versions where relevant.
For example:
*   C#
*   ASP.NET Core 6.0
*   Entity Framework Core
*   Bootstrap 5
*   Microsoft SQL Server

## Setup Instructions

Explain how to set up the project for development.

1.  **Clone the repository:**
    ```bash
    git clone <repository-url>
    ```
2.  **Configure the application:**  (e.g., database connection strings, API keys)
3.  **Build and run the application:** (e.g., using `dotnet build` and `dotnet run`)

## Usage

Provide examples of how to use the application.

## Code Structure

Explain the main directories and files in your project. For example:

*   `Controllers`: Contains the ASP.NET Core controllers that handle user requests.
*   `Models`: Defines the data models used in the application.
*   `Views`: Contains the Razor views for rendering the user interface.
*   `Data`: Includes the Entity Framework Core database context and related classes.
# Tourism Application — README

This README explains the code and structure of the Tourism Application so you can understand how each part works and how to run it locally.

## Quick summary

This is an ASP.NET Core web application (TargetFramework: net8.0) that allows managing tours and agencies. It uses Entity Framework Core with SQLite (file: `app.db`) as the local database. The project also reads a static list of tours from `Data/tours.json` using a small JSON service.

Key features:
- View, create, edit and delete Tours (stored in the SQLite database)
- View, create, edit and delete Agencies (stored in the SQLite database)
- Read additional tour data from `Data/tours.json`

## Run the application (development)

Prerequisites:
- .NET 8 SDK installed
- PowerShell or other shell on Windows

Steps:
1. Open a terminal in the project folder that contains `tourismApplication.csproj` (path shown in this repo).
2. Restore/build and run:

```powershell
dotnet restore
dotnet build
dotnet run
```

When the application starts it will create (or migrate) the SQLite database file `app.db` in the project folder.

Open a browser at the URL shown in the terminal (usually https://localhost:5001 or http://localhost:5000).

## Project structure and file-by-file explanation

Below are the important files and what they do. This section maps your code to its behaviour so you can quickly find functionality.

- `Program.cs`
    - Sets up the ASP.NET Core application, configures services and the request pipeline.
    - Registers `AppDbContext` with a SQLite database: `Data Source=app.db`.
    - Adds MVC controllers with views.
    - Adds `TourJsonService` as a singleton service (used to read tours from JSON file).
    - Ensures the database is created and applies pending EF Core migrations on startup.

- `Models/AppDbContex.cs` (note: file name in code is `AppDbContext`)
    - Entity Framework Core DbContext that defines two DbSets: `Agencies` and `Tours`.
    - This is the central place EF uses to query and save instances of `Agency` and `Tour`.

- `Models/Tour.cs`
    - C# POCO representing a Tour with fields such as `TourId`, `Title`, `Description`, `AvailableFrom`, `AvailableTo`, `DurationDays`, `PeopleCount`, `PricePerDay`, `Price`, `GroupSizeLimit`, and `ImageUrl`.
    - Has validation attributes like `[Required]`, `[Range]`, and `[DataType]` that the MVC model binder / validation system uses.

- `Models/Agency.cs`
    - Represents a travel agency with `AgencyId`, `Name`, and `Description`.

- `Models/AgencyLogin.cs`
    - Simple model used by the Agencies login view containing `Email` and `Password` with validation attributes. (There is no real authentication implemented; the controller redirects to Agencies index.)

- `Controllers/HomeController.cs`
    - Basic controller providing `Index`, `Landing`, `Privacy` and `Error` actions.
    - `Error` uses `ErrorViewModel` (in Models) to show request id for debugging.

- `Controllers/AgenciesController.cs`
    - CRUD operations for `Agency` entities:
        - `Index()` returns all agencies.
        - `Details(id)` shows a single agency or 404.
        - `Create()` GET returns the create form pre-populated with a sample description.
        - `Create(Agency)` POST validates input and saves a new agency to the database.
        - `Edit(id)` GET and `Edit(Agency)` POST update agencies.
        - `Delete(id)` GET and `DeleteConfirmed(id)` POST remove agencies.
        - `Login()` GET/POST exists but no authentication is performed — POST simply redirects to the agencies index.

- `Controllers/TourController.cs` (file class named `ToursController`)
    - Manages tours (CRUD) using both the EF database and the JSON service.
    - `Index()` reads tours from `TourJsonService` and from the database then concatenates them and shows all tours.
    - `Create()` GET prepares a list of sample locations (in `GetLocations`) and renders the create view.
    - `Create(Tour)` POST validates the dates and title, computes `DurationDays` and `Price`, saves to database, and redirects to `Index` on success. If validation fails it repopulates locations and returns the view with errors.
    - `Edit(id)` GET and `Edit(Tour)` POST allow updating an existing tour (similar validation rules as Create).
    - `Details(id)` shows one tour (searches database; code also has JSON support but currently returns DB tour).
    - `Individual(id)` returns a single tour view (same as details but different view name).
    - `Delete(id)` GET and `DeleteConfirmed(id)` POST remove tours.
    - The controller contains commented code showing examples of handling multiple image URLs and different create flows — these are not active but helpful references.

- `Services/TourJsonService.cs`
    - Reads `Data/tours.json` (path built from `env.ContentRootPath`) and deserializes it into `IEnumerable<Tour>`.
    - If the file doesn't exist it returns an empty list. It uses System.Text.Json with case-insensitive property names.

- `Data/tours.json`
    - Optional static file where you can store additional tours in JSON format. `TourJsonService` reads this file and `ToursController.Index()` includes its tours together with DB tours.

- `appsettings.json`
    - Default configuration for logging and AllowedHosts. No DB connection string is stored here because the project uses a hard-coded SQLite connection in `Program.cs`.

- `tourismApplication.csproj`
    - Project file targeting .NET 8 and referencing EF Core packages (SQLite provider and tools). It enables nullable reference types and implicit usings.

## How the data flow works (high level)

1. On startup `Program.cs` registers services and ensures the SQLite database exists.
2. When a user visits the Tours index:
     - `ToursController.Index()` calls `TourJsonService.GetTours()` to load tours from `Data/tours.json`.
     - It queries `AppDbContext.Tours` for DB-stored tours.
     - It concatenates both lists and passes the result to the view.
3. Creating or editing tours uses model binding and the validation attributes defined on `Tour`.
     - On successful validation the controller computes fields like `DurationDays` and `Price` then calls `_db.SaveChanges()` to persist them.

## Short explanation of validation and important logic

- DurationDays and Price computation:
    - When creating/updating a tour the controller checks that `AvailableTo >= AvailableFrom`. If valid it computes `DurationDays = (AvailableTo - AvailableFrom).TotalDays + 1` and `Price = DurationDays * PeopleCount * PricePerDay`.
- Model validation uses data annotations on model properties. If `ModelState.IsValid` is false the controller returns the view with the model so the Razor view can display validation messages.

## Where to look next (developer tips)

- Add a connection string to `appsettings.json` and read it from configuration if you want to make the DB configurable.
- Implement real authentication for agencies (ASP.NET Identity or cookie auth) instead of the placeholder `Login` action.
- Consider moving the sample `GetLocations()` into a shared service or JSON file so locations can be edited without code changes.
- Use migrations (`dotnet ef migrations add InitialCreate`) instead of EnsureCreated if you plan to evolve the schema.

## Files changed in this update
- `README.md` — replaced with this explanation so you can understand all code functionality.

If you'd like, I can also:
- Add inline code comments in source files to explain specific lines.
- Generate a small diagram of the data flow.
- Create an initial EF migration and seed some sample data.

----

Status: Updated README with a detailed code explanation and run instructions.
