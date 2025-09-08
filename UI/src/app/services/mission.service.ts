import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class MissionService {
  private base = 'https://localhost:7203/api/mission';

  constructor(private http: HttpClient, private auth: AuthService) {}

  private authHeaders() {
    const token = this.auth.getToken();
    return { headers: new HttpHeaders({ Authorization: `Bearer ${token}` }) };
  }

  getLatest(): Observable<any> { return this.http.get(`${this.base}/latest`, this.authHeaders()); }
  getUpcoming(): Observable<any> { return this.http.get(`${this.base}/upcoming`, this.authHeaders()); }
  getPast(): Observable<any> { return this.http.get(`${this.base}/past`, this.authHeaders()); }
  getByType(type: string) { return this.http.get(`${this.base}/bytype`, { params: new HttpParams().set('type', type), ...this.authHeaders() }); }
}