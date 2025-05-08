import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';

/**
 * Response from login API endpoint
 */
export interface LoginResponse {
  message: string;
  success: boolean;
  token: string;
}

/**
 * Response from register API endpoint
 */
export interface RegisterResponse {
  message: string;
  success: boolean;
}

/**
 * User model containing profile information
 */
export interface User {
  username: string;
  email: string;
  firstName: string;
  lastName: string;
}

/**
 * Authentication service handling user login, registration and token management
 */
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  // API endpoint base URL
  private apiUrl = 'http://localhost:5004/api/auth';
  
  // BehaviorSubject to track the current user throughout the application
  private currentUserSubject: BehaviorSubject<User | null>;
  
  // Observable for components to subscribe to user changes
  public currentUser$: Observable<User | null>;

  constructor(
    private http: HttpClient,
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    // Initialize with saved user data, if any
    const storedUser = this.getStoredUser();
    this.currentUserSubject = new BehaviorSubject<User | null>(storedUser);
    this.currentUser$ = this.currentUserSubject.asObservable();
  }

  /**
   * Safely access localStorage only if running in the browser
   * This prevents SSR issues with localStorage not being available
   */
  private get storage(): Storage | null {
    return isPlatformBrowser(this.platformId) ? localStorage : null;
  }

  /**
   * Log in the user with email and password
   * @param email User's email address
   * @param password User's password
   * @returns Observable of LoginResponse
   */
  login(email: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, { email, password }).pipe(
      tap(response => {
        if (response.success && response.token) {
          // Save JWT token to localStorage
          this.setToken(response.token);
          
          // Fetch user details using token
          this.fetchCurrentUser().subscribe({
            next: user => {
              this.setStoredUser(user);
              this.currentUserSubject.next(user);
              this.router.navigate(['/home']);
            },
            error: () => {
              this.logout(); // If user fetch fails, logout for security
            }
          });
        }
      })
    );
  }

  /**
   * Register a new user
   * @param firstName User's first name
   * @param lastName User's last name
   * @param username User's username
   * @param email User's email address
   * @param password User's password
   * @returns Observable of RegisterResponse
   */
  register(
    firstName: string,
    lastName: string,
    username: string,
    email: string,
    password: string
  ): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(`${this.apiUrl}/register`, {
      firstName,
      lastName,
      username,
      email,
      password
    });
  }

  /**
   * Fetch current authenticated user details from the API
   * @returns Observable of User
   * @private
   */
  private fetchCurrentUser(): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/me`);
  }

  /**
   * Log out the current user and clear authentication data
   */
  logout(): void {
    this.removeToken();
    this.removeStoredUser();
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }

  /**
   * Store JWT token in localStorage
   * @param token JWT token string
   * @private
   */
  private setToken(token: string): void {
    this.storage?.setItem('auth_token', token);
  }

  /**
   * Get JWT token from localStorage
   * @returns JWT token or null if not found
   */
  getToken(): string | null {
    return this.storage?.getItem('auth_token') ?? null;
  }

  /**
   * Remove JWT token from localStorage
   * @private
   */
  private removeToken(): void {
    this.storage?.removeItem('auth_token');
  }

  /**
   * Store user object in localStorage
   * @param user User object to store
   * @private
   */
  private setStoredUser(user: User): void {
    try {
      this.storage?.setItem('auth_user', JSON.stringify(user));
    } catch (e) {
      console.error('Error storing user:', e);
    }
  }

  /**
   * Get user object from localStorage
   * @returns User object or null if not found or invalid
   * @private
   */
  private getStoredUser(): User | null {
    try {
      const json = this.storage?.getItem('auth_user');
      return json ? JSON.parse(json) as User : null;
    } catch (e) {
      console.warn('Failed to parse stored user:', e);
      return null;
    }
  }

  /**
   * Remove user object from localStorage
   * @private
   */
  private removeStoredUser(): void {
    this.storage?.removeItem('auth_user');
  }

  /**
   * Check if user is currently authenticated (has token)
   * @returns boolean indicating if user is authenticated
   */
  isAuthenticated(): boolean {
    return !!this.getToken();
  }
}