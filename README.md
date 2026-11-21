# SipSmart: Wine Recommendation App

[![CI](https://github.com/rogerscaleb78-art/Wine-Recommendation-App/actions/workflows/ci.yml/badge.svg)](https://github.com/rogerscaleb78-art/Wine-Recommendation-App/actions/workflows/ci.yml)

An ASP.NET Core MVC web app that suggests wines based on what the user types: their mood, the weather, the food they're serving, or the occasion. Users can browse a wine catalog, keep a history of their recommendations, and manage wines through a full CRUD interface.

Built as a team project (November 2024) and later extended with automated tests and continuous integration.

## Highlights

- **C# backend on .NET 8**: ASP.NET Core MVC with Entity Framework Core and SQLite
- **Recommendation engine**: rule-based keyword matching across four categories (mood, weather, food pairing, occasion), isolated in its own service class so it can be unit tested
- **Soft deletes**: removed prompts are flagged rather than erased, so recommendation history is preserved
- **Authentication**: ASP.NET Core Identity, with optional Google, Facebook and Microsoft sign-in
- **Automated tests**: xUnit tests covering the recommendation engine and the controllers, run against an in-memory SQLite database
- **CI pipeline**: GitHub Actions builds and tests every push and pull request on Windows, macOS and Linux

## Tech stack

| Area | Tools |
|---|---|
| Language | C# |
| Framework | ASP.NET Core 8 MVC, Razor views |
| Data | Entity Framework Core 8, SQLite |
| Auth | ASP.NET Core Identity, OAuth (Google / Facebook / Microsoft) |
| Front end | Bootstrap 5, jQuery |
| Testing | xUnit, SQLite in-memory |
| CI/CD | GitHub Actions (build + test matrix, Azure deploy workflow) |

## How the recommendations work

`Services/WineRecommender.cs` lowercases the user's text and checks it against keyword lists in four categories. Each category contributes at most one suggestion, so a prompt like *"a hot summer celebration with shrimp"* returns sparkling wine (mood), a crisp white (weather) and a seafood pairing (food). If nothing matches, it falls back to a versatile default (Pinot Noir), and every response ends with a serving-temperature tip.

## Project structure

```
├── GroupProject421-521/          # Web application
│   ├── Controllers/              # Wines, UserPrompts, Users, Home
│   ├── Services/                 # WineRecommender (recommendation logic)
│   ├── Models/                   # Wine, UserPrompt, Users, view models
│   ├── Data/                     # ApplicationDbContext (EF Core)
│   ├── Views/                    # Razor views
│   └── wwwroot/                  # Static assets
├── tests/
│   └── GroupProject421-521.Tests/  # xUnit test project
└── .github/workflows/
    ├── ci.yml                    # Build + test on Windows, macOS, Linux
    └── master_sipsmart.yml       # Deploy to Azure App Service
```

## Running locally

**Prerequisites:** [.NET 8 SDK](https://dotnet.microsoft.com/download)

```bash
git clone https://github.com/rogerscaleb78-art/Wine-Recommendation-App.git
cd Wine-Recommendation-App
dotnet run --project GroupProject421-521
```

Then open the URL printed in the terminal (for example `https://localhost:5001`). The SQLite database (`wineapp.db`) is created automatically if it doesn't exist.

### Optional: social sign-in

OAuth providers are only enabled when credentials are configured. Keep them out of source control by using [user secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets):

```bash
cd GroupProject421-521
dotnet user-secrets set "Google:ClientIdKey" "<your-client-id>"
dotnet user-secrets set "Google:SecretKey" "<your-secret>"
dotnet user-secrets set "Authentication:Facebook:AppId" "<your-app-id>"
dotnet user-secrets set "Authentication:Facebook:AppSecret" "<your-app-secret>"
dotnet user-secrets set "Authentication:Microsoft:ClientId" "<your-client-id>"
dotnet user-secrets set "Authentication:Microsoft:ClientSecret" "<your-client-secret>"
```

## Running the tests

```bash
dotnet test
```

The test suite covers:

- **Recommendation engine**: keyword matching for every category, case-insensitivity, combining categories, first-match-wins within a category, the default fallback, and the serving tip
- **WinesController**: details lookups (missing ID, unknown wine, existing wine), creating wines with valid and invalid input, and deleting
- **UserPromptsController**: hiding soft-deleted prompts, newest-first ordering, and soft delete leaving the row in place

Each controller test gets a fresh in-memory SQLite database, so the tests are isolated and need no external services.

## Possible next steps

- Swap the keyword engine for a call to an LLM API, keeping the rule-based engine as a fallback
- Ratings and reviews for wines
- Link saved recommendations to specific wines in the catalog
