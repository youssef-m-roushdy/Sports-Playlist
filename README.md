# 📱 Sports Playlist App

A full-stack web application for tracking and managing sports matches with personalized playlists. This application allows users to browse matches, watch live or replay matches, and create personalized playlists of their favorite matches.

## 🌟 Features

### User Authentication
- **Registration:** Create a new account with email validation
- **Login:** Secure access with JWT authentication  
- **Profile Management:** View and manage your user profile

### Match Management
- **Browse:** View all available matches in a responsive grid layout
- **Filter:** Sort matches by status (Live/Replay)
- **Create:** Add new match entries with details (title, competition, date, URL)
- **Edit:** Update match information for existing entries
- **Delete:** Remove matches with confirmation dialog

### Playlist Functionality
- **Add to Playlist:** Save favorite matches to personal playlist
- **View Playlist:** Access personalized match collection
- **Remove:** Delete matches from playlist

### Match Watching
- **Embedded Player:** Watch matches directly within the application
- **YouTube Integration:** Automatic conversion of YouTube links to embedded format
- **Status Indicator:** Visual indicator showing if a match is live or a replay

### UI Features
- **Responsive Design:** Optimized for mobile, tablet, and desktop
- **Collapsible Sidebar:** Space-efficient navigation
- **Material Design:** Modern UI components with consistent styling
- **Loading States:** Visual feedback during asynchronous operations
- **Form Validation:** Immediate user feedback on input errors

## 🛠️ Technology Stack

### Frontend (Angular)
- **Framework:** Angular 16+
- **UI Components:** Angular Material
- **State Management:** RxJS
- **Routing:** Angular Router
- **HTTP Client:** Angular HttpClient
- **Form Handling:** Angular Reactive Forms
- **Authentication:** JWT with interceptors

### Backend (.NET Core)
- **Framework:** ASP.NET Core 7
- **Database:** SQL Server
- **ORM:** Entity Framework Core
- **Authentication:** ASP.NET Core Identity with JWT
- **API Documentation:** Swagger
- **Validation:** FluentValidation

## 📋 Prerequisites

Before you begin, ensure you have the following installed:
- [Node.js](https://nodejs.org/) (v16+)
- [Angular CLI](https://angular.io/cli) (v16+)
- [.NET SDK](https://dotnet.microsoft.com/download) (v7.0+)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express)

## 🚀 Getting Started

### Clone the Repository

```bash
git clone https://github.com/yourusername/Sports-Playlist.git
cd Sports-Playlist
```

### Backend Setup

1. Navigate to the server directory:
```bash
cd Sports-Playlist-Server
```

2. Create a `appsettings.Development.json` file with your database connection string and JWT settings:
```json
{
  "ConnectionStrings": {
    "MatchesDb": "Server=YOUR_SERVER;Database=SportsPlaylist;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "JWT": {
    "Key": "your-secret-key-with-at-least-16-characters",
    "Issuer": "SportsPlaylist",
    "Audience": "SportsPlaylistUsers",
    "ExpiryInDays": 7
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
```

3. Run database migrations:
```bash
dotnet ef database update
```

4. Start the server:
```bash
dotnet run
```
The API server will be available at `https://localhost:5004`.

### Frontend Setup

1. Navigate to the client directory:
```bash
cd ../Sports-Playlist-Client
```

2. Install dependencies:
```bash
npm install
```

3. Start the Angular development server:
```bash
ng serve
```
The application will be available at `http://localhost:4200`.

## 📚 API Documentation

Once the backend server is running, you can access the Swagger API documentation at:
```
https://localhost:5004/swagger
```

## 📁 Project Structure

```
Sports-Playlist/
├── Sports-Playlist-Client/      # Angular frontend
│   ├── src/
│   │   ├── app/                 # Angular components and services
│   │   │   ├── components/      # UI components
│   │   │   ├── guards/          # Authentication guards
│   │   │   ├── interceptors/    # HTTP interceptors
│   │   │   ├── pipes/           # Custom pipes
│   │   │   └── services/        # Services for API communication
│   │   ├── assets/              # Static assets
│   │   ├── environments/        # Environment configurations
│   │   └── styles.css           # Global styles
│
└── Sports-Playlist-Server/      # .NET Core backend
    ├── Controllers/             # API controllers
    ├── Data/                    # Database context
    ├── DTOs/                    # Data transfer objects
    ├── Extensions/              # Extension methods
    ├── Interfaces/              # Interfaces for services
    ├── Mappers/                 # Object mappers
    ├── Migrations/              # EF Core migrations
    ├── Models/                  # Database entities
    ├── Repositories/            # Data access layer
    ├── Services/                # Business logic
    └── Validators/              # Input validation
```

## 🧪 Running Tests

### Frontend Tests
```bash
cd Sports-Playlist-Client
ng test
```

### Backend Tests
```bash
cd Sports-Playlist-Server
dotnet test
```

## 🔒 Authentication Flow

1. User registers with email, password, and personal details
2. User logs in with credentials
3. Backend validates credentials and returns JWT token
4. Frontend stores token in localStorage
5. HTTP interceptors attach token to subsequent API requests
6. Protected routes use auth guards to verify authentication

## 📱 Responsive Design

The application is fully responsive and works on:
- **Desktop:** Full featured experience with expanded sidebar
- **Tablets:** Adaptive layout with collapsible sidebar
- **Mobile Phones:** Optimized compact view with stacked elements
