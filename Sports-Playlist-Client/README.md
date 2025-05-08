# Sports Playlist - Client Application

This is the frontend application for the Sports Playlist project, built with Angular.

## Application Structure

### Core Components

- **Home Component**: Dashboard that displays matches and provides filtering options
- **Match Form Component**: Create and edit match entries
- **Watch Component**: Video player for watching match content
- **Authentication Components**: Login and registration forms

### Services

- **Auth Service**: Handles user authentication, registration, and token management
- **Match Service**: Manages CRUD operations for match data
- **Playlist Service**: Handles adding/removing matches from user playlists

### Guards & Interceptors

- **Auth Guard**: Protects routes that require authentication
- **Auth Interceptor**: Automatically attaches JWT tokens to API requests

## Development Guidelines

### Component Structure

Each component should:
- Have a clear, single responsibility
- Use reactive forms for user input
- Include proper error handling
- Have responsive design for all screen sizes

### Styling

- Use Angular Material components for consistency
- Follow BEM methodology for custom CSS classes
- Define shared styles in `styles.css`
- Use variables for colors and spacing

### State Management

- Use RxJS for reactive programming
- Maintain data in services when possible
- Use BehaviorSubjects for shared state

## Folder Structure

```
src/
├── app/
│   ├── components/              # UI components
│   │   ├── auth/                # Authentication components
│   │   │   ├── login/
│   │   │   └── register/
│   │   ├── confirmation-dialog/ # Reusable confirmation dialog
│   │   ├── home/                # Main dashboard
│   │   ├── match-form/          # Create/edit match form
│   │   └── watch/               # Video player component
│   ├── guards/                  # Route guards
│   ├── interceptors/            # HTTP interceptors
│   ├── pipes/                   # Custom pipes
│   ├── services/                # Data services
│   ├── app.component.ts         # Root component
│   ├── app.config.ts            # App configuration
│   └── app.routes.ts            # Route definitions
├── assets/                      # Static assets
└── styles.css                   # Global styles
```

## Available Scripts

- `ng serve`: Start development server
- `ng build`: Build production app
- `ng test`: Run unit tests
- `ng lint`: Run linting
- `ng generate component [name]`: Generate new component

## APIs Used

The application communicates with the backend through these primary endpoints:

- `/api/auth`: Authentication endpoints
- `/api/matches`: Match management endpoints
- `/api/playlist`: Playlist management endpoints

## Style Guide

1. Use descriptive variable and function names
2. Add JSDoc comments for functions and components
3. Handle errors gracefully with user feedback
4. Organize imports by source (Angular, Material, App)
5. Keep components small and focused on a single responsibility
