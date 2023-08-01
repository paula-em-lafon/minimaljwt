import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { of, Observable, BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
import { Location } from '@angular/common' 

const USER_KEY= 'currentuser';
const REFRESH_TOKEN = 'refreshToken';
const JWT_TOKEN = 'Authorization';


@Injectable({
  providedIn: 'root'
})
export class StorageService {

  private isLoggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(this.isLoggedInfunc()); // initial value is "userdoc is not ready"
 
  private LastPage: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);

  private LastPageThis: string | null = null;

  constructor(private cookieService: CookieService, private jwtHelper: JwtHelperService, public router: Router, public location: Location) { }
  clean(): void {
    window.sessionStorage.clear();
    this.cookieService.delete(JWT_TOKEN);
  }

  public saveUser(user: any): void {
    this.getValueLastPage().subscribe(value => {
      this.LastPageThis = value;
    });
    this.cookieService.delete(JWT_TOKEN)
    this.cookieService.set(JWT_TOKEN, user.token);
    delete user.token;
    window.localStorage.removeItem(REFRESH_TOKEN);
    window.localStorage.setItem(REFRESH_TOKEN, user.refreshToken);
    delete user.refreshToken;
    window.localStorage.setItem(USER_KEY, JSON.stringify(user));
    this.setValueLoggedIn(true);
    console.log(this.router.url)
    if (this.router.url.includes("/needlogin") && this.LastPageThis !== null) {
      console.log(this.LastPageThis);
      this.navigateback(this.LastPageThis);
    } else {
      this.router.navigate(['/'])
    }

  }

  private navigateback(page: string): void {
    console.log(page)
    this.router.navigate([page], { replaceUrl: true })
  }

  public getUser(): any {
    const user = window.sessionStorage.getItem(USER_KEY);
    if (user) {
      return JSON.parse(user);
    }

    return {};
  }

  public isLoggedInfunc(): boolean {
    const token = this.cookieService.get('Authorization');
    if (token) {
      return !this.jwtHelper.isTokenExpired(token);;
    }

    return false;
  }

  public getRefreshToken(): any {
    const user = window.localStorage.getItem(REFRESH_TOKEN);
    if (user) {
      return user;
    }

    return null;
  }

  public getToken(): any {
    const token = this.cookieService.get('Authorization')
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

  setValueLastPage(value: string) {
    this.LastPage.next(value);
  }

  getValueLastPage() {
    return this.LastPage.asObservable();
  }
}
