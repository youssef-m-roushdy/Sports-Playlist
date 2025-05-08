import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';

export interface LoginResponse {
  message: string;
  success: boolean;
  token: string;
}

export interface RegisterResponse {
  message: string;
  success: boolean;
}

export interface User {
  username: string;
  email: string;
  firstName: string;
  lastName: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5004/api/auth';
  private currentUserSubject: BehaviorSubject<User | null>;
  public currentUser$: Observable<User | null>;

  constructor(
    private http: HttpClient,
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    const storedUser = this.getStoredUser();
    this.currentUserSubject = new BehaviorSubject<User | null>(storedUser);
    this.currentUser$ = this.currentUserSubject.asObservable();
  }

  /** Safely access localStorage only if running in the browser */
  private get storage(): Storage | null {
    return isPlatformBrowser(this.platformId) ? localStorage : null;
  }

  /** Login the user and store token + user data */
  login(email: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, { email, password }).pipe(
      tap(response => {
        if (response.success && response.token) {
          this.setToken(response.token);
          this.fetchCurrentUser().subscribe({
            next: user => {
              this.setStoredUser(user);
              this.currentUserSubject.next(user);
              this.router.navigate(['/home']);
            },
            error: () => {
              this.logout(); // if user fetch fails, logout
            }
          });
        }
      })
    );
  }

  /** Register a new user */
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

  /** Fetch current user from the backend using auth token */
  private fetchCurrentUser(): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/me`);
  }

  /** Logout the user and clear all stored data */
  logout(): void {
    this.removeToken();
    this.removeStoredUser();
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }

  /** Store JWT token in localStorage */
  private setToken(token: string): void {
    this.storage?.setItem('auth_token', token);
  }

  /** Get JWT token from localStorage */
  getToken(): string | null {
    return this.storage?.getItem('auth_token') ?? null;
  }

  /** Remove JWT token from localStorage */
  private removeToken(): void {
    this.storage?.removeItem('auth_token');
  }

  /** Store user object in localStorage */
  private setStoredUser(user: User): void {
    try {
      this.storage?.setItem('auth_user', JSON.stringify(user));
    } catch (e) {
      console.error('Error storing user:', e);
    }
  }

  /** Get user object from localStorage */
  private getStoredUser(): User | null {
    try {
      const json = this.storage?.getItem('auth_user');
      return json ? JSON.parse(json) as User : null;
    } catch (e) {
      console.warn('Failed to parse stored user:', e);
      return null;
    }
  }

  /** Remove user object from localStorage */
  private removeStoredUser(): void {
    this.storage?.removeItem('auth_user');
  }

  /** Return true if token exists */
  isAuthenticated(): boolean {
    return !!this.getToken();
  }
}