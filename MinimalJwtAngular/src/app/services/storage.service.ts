import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { of, Observable, BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
import { Location } from '@angular/common'

const USER_KEY= 'currentuser';
const REFRESH_TOKEN = 'refreshToken';
const JWT_TOKEN = 'Authorization';
const ADMIN = "Administrator";


@Injectable({
  providedIn: 'root'
})
export class StorageService {

  private isLoggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(this.isLoggedInfunc()); // initial value is "userdoc is not ready"
  private isAdmin: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(this.isAdminFunc());

  private LastPage: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);

  constructor(private cookieService: CookieService, private jwtHelper: JwtHelperService, public router: Router, public location: Location) { }
  clean(): void {
    window.localStorage.clear();
    this.cookieService.delete(JWT_TOKEN, '/')
    this.setValueLoggedIn(false);
  }

  public saveUser(user: any): void {
    this.cookieService.delete(JWT_TOKEN, '/')
    this.cookieService.set(JWT_TOKEN, user.token, {
      path: '/',
      sameSite: 'Strict'
    });
    delete user.token;
    window.localStorage.removeItem(REFRESH_TOKEN);
    window.localStorage.setItem(REFRESH_TOKEN, user.refreshToken);
    delete user.refreshToken;
    window.localStorage.setItem(USER_KEY, JSON.stringify(user));

    this.setValueLoggedIn(true);
  }

  public getUser(): any {
    const user = window.localStorage.getItem(USER_KEY);
    if (user) {
      return JSON.parse(user);
    }

    return {};
  }

  public isLoggedInfunc(): boolean {
    const token = this.cookieService.get(JWT_TOKEN);
    if (token) {
      return !this.jwtHelper.isTokenExpired(token);
    }

    return false;
  }

  isAdminFunc(): boolean {
    let userData = this.getUser();
    if (userData.role && userData.role === ADMIN) {
      return true;
    }
    else {
      return false;
    }
  }

  public getRefreshToken(): any {
    const user = window.localStorage.getItem(REFRESH_TOKEN);
    if (user) {
      return user;
    }

    return null;
  }

  public getToken(): any {
    const token = this.cookieService.get(JWT_TOKEN)
    if (token) {
      return token;
    }
    return null;
  }

  setValueLoggedIn(value: boolean) {
    this.isLoggedIn.next(value);
  }

  getValueLoggedIn() {
    return this.isLoggedIn.asObservable();
  }

  setValueIsAdmin(value: boolean) {
    this.isAdmin.next(value);
  }

  getValueIsAdmin() {
    return this.isAdmin.asObservable();
  }

  setValueLastPage(value: string | null) {
    this.LastPage.next(value);
  }

  getValueLastPage() {
    return this.LastPage.asObservable();
  }
}
