import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { StorageService } from './storage.service';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';
import { Location } from '@angular/common';
/*import { RouterExtService } from './router-ext-service.service';*/

@Injectable({
  providedIn: 'root'
})
export class AdminguardService {
  constructor(public auth: AuthService, public storage: StorageService, public router: Router, private location: Location
    /*, private routereExt: RouterExtService*/
  ) { }

  async canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    let user = this.storage.getUser();
    if (this.storage.isAdminFunc()) {
      return true
    }
    else {
      this.router.navigate(['needadmin']);
      return false
    }
  }
}
