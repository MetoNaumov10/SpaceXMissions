import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

interface AuthResponse { token: string; expiresAt: string }

@Injectable({ providedIn: 'root' })
export class AuthService {
  private base = 'https://localhost:7203/api/auth';

  constructor(private http: HttpClient) {}

  signup(data: { firstName: string; lastName: string; email: string; password: string }) {
    return this.http.post(`${this.base}/signup`, data);
  }

  login(data: { email: string; password: string }): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.base}/login`, data).pipe(
      tap(res => this.setToken(res.token))
    );
  }

  setToken(token: string) { localStorage.setItem('token', token); }
  getToken(): string | null { return localStorage.getItem('token'); }
  isAuthenticated() { return !!this.getToken(); }
  logout() { localStorage.removeItem('token'); }
}