import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  accessToken: string;
  learnerId: number;
  email: string;
}

export interface CurrentUser {
  learnerId: number;
  email: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = 'http://localhost:5087/api/auth';

  constructor(private http: HttpClient) {}

login(request: LoginRequest): Observable<LoginResponse> {
  return this.http.post<LoginResponse>(
    `${this.apiUrl}/login`,
    request,
    { withCredentials: true }
  );
}

getCurrentUser(): Observable<CurrentUser> {
  return this.http.get<CurrentUser>(
    `${this.apiUrl}/me`
  );
}
}