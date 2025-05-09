import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { trigger, transition, style, animate } from '@angular/animations';

@Component({
  selector: 'app-welcome',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.css'],
  animations: [
    trigger('fadeIn', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(20px)' }),
        animate('0.5s ease-out', style({ opacity: 1, transform: 'translateY(0)' }))
      ])
    ])
  ]
})
export class WelcomeComponent implements OnInit {
  imgLoaded = false;
  imgError = false;

  features = [
    {
      icon: 'sports_soccer',
      title: 'Watch Live Matches',
      description: 'Stream live sports events as they happen in real-time'
    },
    {
      icon: 'playlist_play',
      title: 'Create Playlists',
      description: 'Build your personal collection of favorite matches'
    },
    {
      icon: 'replay',
      title: 'Replay Games',
      description: 'Missed a game? Watch replays at your convenience'
    },
    {
      icon: 'devices',
      title: 'Multi-platform',
      description: 'Enjoy on any device - desktop, tablet, or mobile'
    }
  ];

  ngOnInit() {
    // Preload the hero image to check if it loads correctly
    const img = new Image();
    img.onload = () => {
      this.imgLoaded = true;
      this.imgError = false;
    };
    img.onerror = () => {
      this.imgLoaded = false;
      this.imgError = true;
      console.error('Failed to load hero image');
    };
    img.src = 'assets/images/hero-image.svg';
  }
}
