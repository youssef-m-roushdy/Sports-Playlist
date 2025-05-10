# Sports Playlist - Client Application

This is the frontend application for the Sports Playlist project, built with Angular.

## Features

- **Live Match Streaming**: Watch sports events as they happen in real-time
- **Personalized Playlists**: Create collections of your favorite matches
- **Match Replay**: Watch past matches at your convenience
- **Multi-device Support**: Enjoy on desktop, tablet, or mobile devices
- **User Authentication**: Secure registration and login process
- **Responsive Design**: Optimized viewing experience on all screen sizes

## Getting Started

### Prerequisites

- Node.js (v16.x or later)
- npm (v8.x or later)
- Angular CLI (v19.x)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/username/Sports-Playlist.git
   cd Sports-Playlist/Sports-Playlist-Client
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Configure environment variables:
   - Create `src/environments/environment.ts` for development
   - Set API URL and other configuration variables

4. Start development server:
   ```bash
   ng serve
   ```

5. Open your browser to `http://localhost:4200`

## Application Structure

### Core Components

- **Home Component**: Dashboard that displays matches and provides filtering options
- **Match Form Component**: Create and edit match entries
- **Watch Component**: Video player for watching match content
- **Authentication Components**: Login and registration forms
- **Welcome Component**: Landing page with feature highlights

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
- Implement animations for better UX

### Styling

- Use Angular Material components for consistency
- Follow BEM methodology for custom CSS classes
- Define shared styles in `styles.css`
- Use variables for colors and spacing
- Implement media queries for responsive design

### State Management

- Use RxJS for reactive programming
- Maintain data in services when possible
- Use BehaviorSubjects for shared state
- Implement proper error handling with catchError operators

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
