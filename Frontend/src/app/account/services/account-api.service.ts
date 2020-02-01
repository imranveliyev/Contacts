import { Injectable } from '@angular/core';
import { AccountCredentials } from '../models/account-credentials';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Account } from "../models/account";

import { environment } from "../../../environments/environment";
import { AuthResponse } from '../models/auth-response';

@Injectable({
  providedIn: 'root'
})
export class AccountApiService {

  private readonly apiUrl: string = environment.authApiUrl;

  constructor(private httpClient: HttpClient) { }

  register(credentials: AccountCredentials) : Observable<void> {
    return this.httpClient.post<void>(`${this.apiUrl}/register`, credentials);
  }

  login(credentials: AccountCredentials) : Observable<AuthResponse> {
    return this.httpClient.post<AuthResponse>(`${this.apiUrl}/login`, credentials);
  }

  refresh(refreshToken: string) : Observable<AuthResponse> {
    return this.httpClient.get<AuthResponse>(`${this.apiUrl}/refresh/${refreshToken}`);
  }

  logout(refreshToken: string) : Observable<void> {
    return this.httpClient.get<void>(`${this.apiUrl}/logout/${refreshToken}`);
  }

  search(username: string) : Observable<Account> {
    return this.httpClient.get<Account>(`${this.apiUrl}/search/${username}`);
  }

  getAll() : Observable<Array<Account>> {
    return this.httpClient.get<Array<Account>>(`${this.apiUrl}`);
  }
}
