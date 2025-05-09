import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterModule],
  template: `
    <router-outlet></router-outlet>
  `,
  styles: []
})
export class AppComponent {
  title = 'Sports Playlist';
}