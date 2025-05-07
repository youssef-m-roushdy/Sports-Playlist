import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MatchService } from '../../services/match.service';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Component({
  selector: 'app-watch',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule],
  templateUrl: './watch.component.html',
  styleUrls: ['./watch.component.css']
})
export class WatchComponent implements OnInit {
  match: any = null;
  safeUrl: SafeResourceUrl | null = null;
  loading = true;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private matchService: MatchService,
    private sanitizer: DomSanitizer,
    private router: Router
  ) {}

  navigateHome() {
    this.router.navigate(['/']);
  }

  ngOnInit() {
    const matchId = this.route.snapshot.paramMap.get('id');
    if (matchId) {
      this.loadMatchDetails(+matchId);
    } else {
      this.error = 'No match ID provided';
      this.loading = false;
    }
  }

  loadMatchDetails(matchId: number) {
    this.matchService.getMatchById(matchId).subscribe({
      next: (match) => {
        this.match = match;
        const embedUrl = this.convertToEmbedUrl(match.matchUrl);
        this.safeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(embedUrl);
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load match details';
        this.loading = false;
        console.error(err);
      }
    });
  }
  
  private convertToEmbedUrl(url: string): string {
    const youtubeRegex = /(?:https?:\/\/)?(?:www\.)?youtube\.com\/watch\?v=([^\s&]+)/;
    const match = url.match(youtubeRegex);
    if (match && match[1]) {
      return `https://www.youtube.com/embed/${match[1]}`;
    }
    return url; // fallback if it's already an embed URL or not a YouTube URL
  }

  openInNewTab() {
    if (this.match?.matchUrl) {
      window.open(this.match.matchUrl, '_blank');
    }
  }
}