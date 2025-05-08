import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

/**
 * Match model representing a sports match
 */
export interface Match {
  id?: number;          // Optional for create operations
  title: string;        // Match title (e.g., "Team A vs Team B")
  competition: string;  // Competition name (e.g., "Premier League")
  matchDate: Date;      // Date and time of the match
  status: 'Live' | 'Replay'; // Current match status
  matchUrl: string;     // URL to the match video
}

/**
 * Service for managing sports matches in the application
 */
@Injectable({
  providedIn: 'root'
})
export class MatchService {
  private apiUrl = 'http://localhost:5004/api/matches';

  constructor(private http: HttpClient) {}

  /**
   * Get all matches from the API
   * @returns Observable of Match array
   */
  getAllMatches(): Observable<Match[]> {
    return this.http.get<Match[]>(this.apiUrl);
  }

  /**
   * Get matches filtered by status (Live or Replay)
   * @param status Match status to filter by
   * @returns Observable of filtered Match array
   */
  getMatchesByStatus(status: string): Observable<Match[]> {
    return this.http.get<Match[]>(`${this.apiUrl}/status?status=${status}`);
  }

  /**
   * Get a specific match by its ID
   * @param id Match ID
   * @returns Observable of Match
   */
  getMatchById(id: number): Observable<Match> {
    return this.http.get<Match>(`${this.apiUrl}/${id}`);
  }

  /**
   * Get matches in the current user's playlist
   * @returns Observable of Match array
   */
  getUserPlaylist(): Observable<Match[]> {
    return this.http.get<Match[]>(`${this.apiUrl}/userPlaylist`);
  }

  /**
   * Create a new match
   * @param matchData Match data without ID
   * @returns Observable of created Match
   */
  createMatch(matchData: Match): Observable<Match> {
    return this.http.post<Match>(this.apiUrl, matchData);
  }

  /**
   * Update an existing match
   * @param id Match ID to update
   * @param matchData Updated match data
   * @returns Observable of updated Match
   */
  updateMatch(id: number, matchData: Match): Observable<Match> {
    return this.http.put<Match>(`${this.apiUrl}/${id}`, matchData);
  }

  /**
   * Delete a match
   * @param id Match ID to delete
   * @returns Observable of HTTP response
   */
  deleteMatch(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}