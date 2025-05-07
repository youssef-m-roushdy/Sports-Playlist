import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Match {
  id: number;
  title: string;
  competition: string;
  matchDate: Date;
  status: 'Live' | 'Replay';
  matchUrl: string;
}

@Injectable({
  providedIn: 'root'
})
export class MatchService {
  private apiUrl = 'http://localhost:5004/api/matches';

  constructor(private http: HttpClient) {}

  getAllMatches(): Observable<Match[]> {
    return this.http.get<Match[]>(this.apiUrl);
  }

  getMatchesByStatus(status: string): Observable<Match[]> {
    return this.http.get<Match[]>(`${this.apiUrl}/status?status=${status}`);
  }

  getMatchById(id: number): Observable<Match> {
    return this.http.get<Match>(`${this.apiUrl}/${id}`);
  }

  getUserPlaylist(): Observable<Match[]> {
    return this.http.get<Match[]>(`${this.apiUrl}/userPlaylist`);
  }

}