import { Injectable } from '@angular/core';
import { Router, CanActivate, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from './auth.service';
import { StorageService } from './storage.service';
import { Location } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class AuthguardService {
 constructor(public auth: AuthService, public storage: StorageService, public router: Router, private location: Location) { }

  async canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    if (!this.storage.getToken() || !this.storage.isLoggedInfunc()) {
      if (this.storage.getToken()) {
        const refreshing = await this.auth.tryRefreshingTokens();
        if (refreshing && this.storage.isLoggedInfunc()) {
          return true;
        }
        else {
          this.storage.setValueLastPage(this.location.path());
          this.router.navigate(['needlogin']);
          return false;
        }
      }
      else {

        this.storage.setValueLastPage(this.location.path());
        this.router.navigate(['needlogin']);
        return false;
      }
    }
    this.storage.setValueLastPage(null);
    return true;
  }
}
