import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

// Environment
import { environment } from '../../../environments/environment';

// Models
import { AuthModel, BaseResponseModel } from '../../shared';

@Injectable()
export class AuthService {
  private BASE_URL = environment.base_url;

  constructor(private http: HttpClient) {}

  // Authentication/Authorization
  login(username: string, password: string): Observable<BaseResponseModel<AuthModel>> {
    const apiUrl = `${this.BASE_URL}auth/login`;
    return this.http.post<BaseResponseModel<AuthModel>>(apiUrl, { username, password });
  }

  getToken(): string {
    return localStorage.getItem(environment.authTokenKey);
  }
}
