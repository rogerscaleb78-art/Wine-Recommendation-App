# Wine App - Wine Recommendation System

**Created:** November 2024  
**Project Type:** ASP.NET Core Web Application  
**Framework:** .NET 8.0

## Overview

Wine App is a comprehensive wine recommendation and management system built with ASP.NET Core MVC. The application allows users to browse wines, receive personalized recommendations, save their preferences, and manage their wine collection. It features user authentication, wine catalog management, and an intelligent suggestion system.

This project was developed as an educational endeavor to expand beyond the fundamentals of web development, exploring advanced concepts including multi-provider authentication, complex data relationships, modern architectural patterns, and real-world application development practices.

## Features

### 🍷 Core Functionality
- **Wine Catalog**: Browse and manage a comprehensive database of wines
- **Wine Recommendations**: AI-powered wine suggestions based on user preferences
- **User Profiles**: Personalized user accounts with saved preferences
- **Saved Suggestions**: Users can save and revisit wine recommendations
- **Admin Panel**: Administrative interface for user and content management

### 🔐 Authentication & Security
- **Multi-Provider Authentication**: Support for Google, Facebook, and Microsoft OAuth
- **Cookie-based Authentication**: Secure session management
- **User Identity Management**: Built on ASP.NET Core Identity
- **Protected Routes**: Secure access to user-specific features

### 📊 Data Management
- **SQLite Database**: Local development database for easy setup
- **Entity Framework Core**: Modern ORM for data access
- **CRUD Operations**: Full Create, Read, Update, Delete functionality for all entities

## Technology Stack

- **Backend**: ASP.NET Core 8.0 MVC
- **Database**: SQLite (development), SQL Server (production ready)
- **ORM**: Entity Framework Core 8.0
- **Authentication**: ASP.NET Core Identity with OAuth providers
- **Frontend**: Razor Pages, Bootstrap 5, jQuery
- **External APIs**: Google OAuth, Facebook OAuth, Microsoft OAuth

## Project Structure

```
GroupProject421-521/
├── Controllers/           # MVC Controllers
│   ├── HomeController.cs     # Main application controller
│   ├── WinesController.cs    # Wine management
│   ├── UserPromptsController.cs # User recommendation prompts
│   └── UsersController.cs    # User management
├── Models/               # Data models
│   ├── Wine.cs              # Wine entity
│   ├── UserPrompt.cs        # User recommendation prompts
│   ├── SavedWine.cs         # Saved wine preferences
│   └── Users.cs             # User entity
├── Views/                # Razor view templates
│   ├── Home/                # Home page views
│   ├── Wines/               # Wine management views
│   ├── UserPrompts/         # User prompt views
│   └── Shared/              # Shared layout and partials
├── Data/                 # Database context
│   └── ApplicationDbContext.cs
├── Areas/                # Identity area for authentication
└── wwwroot/              # Static files (CSS, JS, images)
```

## Key Models

### Wine
- **WineID**: Unique identifier
- **Name**: Wine name
- **Type**: Wine type (Red, White, Rosé, etc.)
- **Region**: Geographic origin
- **Description**: Detailed wine description
- **FlavorProfile**: Taste characteristics
- **Price**: Wine price

### UserPrompt
- **Id**: Unique identifier
- **UserId**: Associated user ID
- **PromptText**: User's wine preference query
- **Preferences**: User's wine preferences
- **WineSuggestion**: Generated wine recommendation
- **CreatedAt**: Timestamp of creation
- **IsDeleted**: Soft delete flag

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code
- Git

### Installation

1. **Clone the repository**
   ```bash
   git clone [repository-url]
   cd Wine-App
   ```

2. **Navigate to the project directory**
   ```bash
   cd GroupProject421-521
   ```

3. **Restore dependencies**
   ```bash
   dotnet restore
   ```

4. **Configure authentication (optional)**
   - Add OAuth credentials to `appsettings.json` or user secrets
   - Configure Google, Facebook, or Microsoft OAuth if desired

5. **Run the application**
   ```bash
   dotnet run
   ```

6. **Access the application**
   - Open browser to `https://localhost:5001` or `http://localhost:5000`

## Configuration

### Authentication Setup
The application supports multiple OAuth providers. Configure them in `appsettings.json`:

```json
{
  "Google": {
    "ClientIdKey": "your-google-client-id",
    "SecretKey": "your-google-secret"
  },
  "Authentication": {
    "Facebook": {
      "AppId": "your-facebook-app-id",
      "AppSecret": "your-facebook-app-secret"
    },
    "Microsoft": {
      "ClientId": "your-microsoft-client-id",
      "ClientSecret": "your-microsoft-client-secret"
    }
  }
}
```

### Database Configuration
The application uses SQLite for local development. The database file (`wineapp.db`) will be created automatically on first run.

## Usage

### For End Users
1. **Register/Login**: Create an account or sign in with OAuth providers
2. **Browse Wines**: Explore the wine catalog with detailed information
3. **Get Recommendations**: Submit preferences to receive wine suggestions
4. **Save Favorites**: Save recommended wines for future reference
5. **View History**: Access previously saved suggestions

### For Administrators
1. **User Management**: View and manage user accounts
2. **Wine Management**: Add, edit, or remove wines from the catalog
3. **System Monitoring**: Access admin dashboard for system overview

## Development Notes

- **Database**: Uses Entity Framework Core with SQLite for development
- **Authentication**: Implements ASP.NET Core Identity with optional OAuth
- **Security**: Includes CSRF protection and secure authentication flows
- **Responsive Design**: Bootstrap-based responsive UI
- **Error Handling**: Comprehensive error handling and logging

## Future Enhancements

- Wine rating and review system
- Advanced recommendation algorithms
- Wine pairing suggestions
- Social features for sharing recommendations
- Mobile application
- Wine inventory tracking

## License

This project is created for educational purposes as part of a group project (GroupProject421-521).

## Support

This is a demonstration project created in November 2024. For questions or issues, please refer to the project documentation or contact the development team.

---

*Note: This application is designed for viewing and demonstration purposes. It showcases modern web development practices using ASP.NET Core, Entity Framework, and modern authentication patterns.*