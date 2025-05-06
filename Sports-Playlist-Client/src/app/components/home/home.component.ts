import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="home-container">
      <h1>Welcome to Your Dashboard</h1>
      
      <div *ngIf="authService.currentUser$ | async as user" class="user-info">
        <p>Name: {{ user.firstName }} {{ user.lastName }}</p>
        <p>Email: {{ user.email }}</p>
        <p>Username: {{ user.username }}</p>
      </div>

      <button (click)="logout()" class="logout-button">Logout</button>
    </div>
  `,
  styles: [`
    .home-container {
      max-width: 800px;
      margin: 2rem auto;
      padding: 2rem;
      background: white;
      border-radius: 8px;
      box-shadow: 0 2px 10px rgba(0,0,0,0.1);
    }
    
    .user-info {
      margin: 2rem 0;
      padding: 1rem;
      background: #f5f7fa;
      border-radius: 4px;
    }
    
    .logout-button {
      background: #e53e3e;
      color: white;
      border: none;
      padding: 0.5rem 1rem;
      border-radius: 4px;
      cursor: pointer;
    }
    
    .logout-button:hover {
      background: #c53030;
    }
  `]
})
export class HomeComponent {
  constructor(public authService: AuthService) {}

  logout() {
    this.authService.logout();
  }
}