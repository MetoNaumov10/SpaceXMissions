import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class MissionService {
  private readonly apiUrl = 'https://localhost:7203/mission';

  constructor(private http: HttpClient) {}

  getLatest(): Observable<any> {
    return this.http.get(`${this.apiUrl}/latest`);
  }

  getUpcoming(): Observable<any> {
    return this.http.get(`${this.apiUrl}/upcoming`);
  }

  getPast(): Observable<any> {
    return this.http.get(`${this.apiUrl}/past`);
  }

  getByType(type: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/bytype?type=${type}`);
  }
}