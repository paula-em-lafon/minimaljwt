import { Injectable } from '@angular/core';
import { Router, CanActivate, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from './auth.service';
import { StorageService } from './storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthguardService {

  constructor(public auth: AuthService, public storage: StorageService, public router: Router) { }

  async canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    console.log("ENTERING THE GUARD")
    if (!this.storage.getToken() || !this.storage.isLoggedInfunc()) {
      console.log("doesn't have token")
      if (this.storage.getToken()) {
        const refreshing = await this.auth.tryRefreshingTokens();
        if (refreshing && this.storage.isLoggedInfunc()) {
          return true;
        }
        else {
          this.storage.setValueLastPage(this.router.url);
          this.router.navigate(['needlogin']);
          console.log("Returning from inside 1");
          return false;
        }
      }
      else {
        this.storage.setValueLastPage(this.router.url);
        this.router.navigate(['needlogin']);
        console.log("Returning from inside 2");
        return false;
      }
    }
    console.log(this.router.url)
    debugger
    console.log("Gets into the true part")
    console.log(state.url);
    return true;
  }
}
