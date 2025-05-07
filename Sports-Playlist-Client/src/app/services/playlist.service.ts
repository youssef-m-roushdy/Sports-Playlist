import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service'; // Make sure path is correct

@Injectable({
  providedIn: 'root',
})
export class PlaylistService {
  private apiUrl = 'http://localhost:5004/api/playlist';

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
  }

  addToPlaylist(matchId: number): Observable<any> {
    const headers = this.getAuthHeaders();
    // Send as raw number value instead of object
    return this.http.post(`${this.apiUrl}/`, matchId, {
      headers
    });
  }

  removeFromPlaylist(matchId: number): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.delete(`${this.apiUrl}/${matchId}`, { headers });
  }
}
