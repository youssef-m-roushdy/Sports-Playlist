import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { MatchService } from '../../services/match.service';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { PlaylistService } from '../../services/playlist.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatSnackBarModule
  ],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  matches: any[] = [];
  userPlaylist: any[] = [];
  sidebarOpen = true;
  activeTab: 'all' | 'live' | 'replay' | 'playlist' = 'all';

  constructor(
    public authService: AuthService,
    private matchService: MatchService,
    private playlistService: PlaylistService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.loadMatches();
    if (this.authService.isAuthenticated()) {
      this.loadUserPlaylist();
    }
  }

  loadMatches() {
    this.matchService.getAllMatches().subscribe(matches => {
      this.matches = matches;
    });
  }

  loadUserPlaylist() {
    this.matchService.getUserPlaylist().subscribe(playlist => {
      this.userPlaylist = playlist;
    });
  }

  filterMatches(status?: 'Live' | 'Replay') {
    if (status) {
      this.matchService.getMatchesByStatus(status).subscribe(matches => {
        this.matches = matches;
        this.activeTab = status.toLowerCase() as 'live' | 'replay';
      });
    } else {
      this.loadMatches();
      this.activeTab = 'all';
    }
  }

  showPlaylist() {
    if (this.authService.isAuthenticated()) {
      this.activeTab = 'playlist';
    }
  }

  addToPlaylist(matchId: number) {
    this.playlistService.addToPlaylist(matchId).subscribe({
      next: () => {
        this.loadUserPlaylist();
        this.showSnackbar('Match added to playlist successfully!', 'success');
      },
      error: (err) => {
        if (err.status === 400) {
          this.showSnackbar('This match is already in your playlist!', 'warn');
        } else {
          this.showSnackbar('An error occurred while adding to playlist', 'error');
        }
      }
    });
  }

  removeFromPlaylist(matchId: number) {
    this.playlistService.removeFromPlaylist(matchId).subscribe({
      next: () => {
        this.loadUserPlaylist();
        this.showSnackbar('Match removed from playlist', 'success');
      },
      error: (err) => {
        console.error(err);
        this.showSnackbar('Failed to remove match from playlist', 'error');
      }
    });
  }

  private showSnackbar(message: string, panelClass: 'success' | 'error' | 'warn') {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      panelClass: [`${panelClass}-snackbar`]
    });
  }

  logout() {
    this.authService.logout();
  }

  get displayedMatches() {
    if (this.activeTab === 'playlist') return this.userPlaylist;
    if (this.activeTab === 'all') return this.matches;
    return this.matches.filter(m => m.status.toLowerCase() === this.activeTab);
  }
}