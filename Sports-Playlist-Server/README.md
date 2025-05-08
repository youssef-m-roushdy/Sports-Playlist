# Sports Playlist - Server Application

This is the backend API for the Sports Playlist project, built with ASP.NET Core.

## Application Structure

### Controllers

- **AuthController**: Handles user authentication and registration
- **MatchesController**: Manages sports match data
- **PlaylistController**: Manages user playlists

### Services

- **JwtService**: Generates and validates JWT tokens
- **Other services**: Various business logic services

### Data Access

- **Repositories**: Data access layer with repository pattern
- **Entity Framework**: ORM for database interactions
- **DTOs**: Data transfer objects for API communication

## Development Guidelines

### API Design

- Follow RESTful principles
- Use appropriate HTTP methods and status codes
- Implement comprehensive error handling
- Document all endpoints with XML comments
- Enforce validation on all incoming data

### Security

- Use JWT for authentication
- Implement authorization on endpoints
- Validate all user input
- Protect sensitive data
- Log security events

### Database

- Use migration-based approach
- Define clear entity relationships
- Use indexes for frequently queried fields
- Implement soft delete where appropriate
- Follow naming conventions

## Project Structure

```
Sports-Playlist-Server/
├── Controllers/             # API controllers
├── Data/                    # Database context
├── DTOs/                    # Data transfer objects
├── Extensions/              # Extension methods
├── Interfaces/              # Service interfaces
├── Mappers/                 # Object mappers
├── Migrations/              # EF Core migrations
├── Models/                  # Database entities
├── Repositories/            # Data access layer
├── Services/                # Business logic
├── Validators/              # Input validation
└── Program.cs               # Application entry point
```

## Database Schema

### Main Entities

- **User**: Application user (extends IdentityUser)
- **Match**: Sports match information
- **Playlist**: User-match associations

### Key Relationships

- User to Playlist: One-to-Many
- Match to Playlist: One-to-Many
- User to Match: Many-to-Many (through Playlist)

## API Endpoints

### Auth Endpoints

- **POST /api/auth/register**: Register a new user
- **POST /api/auth/login**: Authenticate a user
- **GET /api/auth/me**: Get current user details

### Match Endpoints

- **GET /api/matches**: Get all matches
- **GET /api/matches/{id}**: Get a specific match
- **POST /api/matches**: Create a new match
- **PUT /api/matches/{id}**: Update a match
- **DELETE /api/matches/{id}**: Delete a match
- **GET /api/matches/status**: Get matches by status
- **GET /api/matches/userPlaylist**: Get user's playlist matches

### Playlist Endpoints

- **POST /api/playlist**: Add a match to user's playlist
- **DELETE /api/playlist/{matchId}**: Remove a match from user's playlist

## Configuration

The application requires the following configuration in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "MatchesDb": "Server=...;Database=...;..."
  },
  "JWT": {
    "Key": "your-secret-key-with-at-least-16-characters",
    "Issuer": "SportsPlaylist",
    "Audience": "SportsPlaylistUsers",
    "ExpiryInDays": 7
  }
}
```

## Authentication Flow

1. User registers or logs in
2. Server validates credentials and generates JWT token
3. Client stores token and sends it with subsequent requests
4. Server validates token on protected endpoints
5. Token expiry triggers re-authentication

## Error Handling

All API endpoints use consistent error responses:

```json
{
  "success": false,
  "errors": ["Error message 1", "Error message 2"]
}
```

## Validation

Input validation is performed using FluentValidation, with validators defined for:

- User registration
- Match creation and updates
- Playlist operations
