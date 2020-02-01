import { Injectable } from '@angular/core';
import { AccountCredentials } from '../models/account-credentials';
import { AccountApiService } from './account-api.service';
import { AccountStateService } from './account-state.service';
import { TokenStorageService } from './token-storage.service';
import { Account } from '../models/account';
import { tap, map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountFacadeService {

  constructor(
    private accountApi: AccountApiService,
    private accountState: AccountStateService,
    private tokenStorage: TokenStorageService
  ) {}

  get account$() { return this.accountState.account$; }

  isAuthenticated$() : Observable<boolean> {
    return this.account$.pipe(map(account => account ? true : false));
  }

  isAuthenticated() : boolean {
    if(this.tokenStorage.accessToken)
      return true;
    return false;
  }

  register(credentials: AccountCredentials) : Observable<void> {
    return this.accountApi.register(credentials);
  }

  login(credentials: AccountCredentials) : Observable<Account> {
    return this.accountApi.login(credentials).pipe(
      tap(response => this.tokenStorage.setTokens(response.accessToken, response.refreshToken)),
      map(response => ({ id: response.userId, username: response.username })),
      tap(account => this.accountState.setAccount(account))
    );
  }

  logout() : Observable<void> {
    return this.accountApi.logout(this.tokenStorage.refreshToken).pipe(
      tap(() => this.tokenStorage.removeTokens()),
      tap(() => this.accountState.removeAccount())
    );
  }

  search(username: string) : Observable<Account> {
    return this.accountApi.search(username);
  }

  getAll() : Observable<Array<Account>> {
    return this.accountApi.getAll();
  }
}
