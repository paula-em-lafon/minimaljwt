import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { StorageService } from '../../services/storage.service';
import { Observable, Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  form: any = {
    username: null,
    password: null
  }

  private LastPageThis: string | null = null;

  logginSubscription: Subscription;
  isLoggedIn = false;
  isLoginFailed = false;
  isAdmin = false;
  errorMessage = '';
  roles: string[] = [];

  constructor(private authService: AuthService, private storageService: StorageService, public router: Router, public location: Location) { }

  ngOnInit(): void {
    this.storageService.getValueLoggedIn().subscribe(value => {
      this.isLoggedIn = value;
    });
    this.storageService.getValueLastPage().subscribe(value => {
      this.LastPageThis = value;
    });
    this.storageService.getValueIsAdmin().subscribe(value => {
      this.isAdmin = value;
    });
  }

  onSubmit(): void {
    const { username, password } = this.form;

    this.authService.login(username, password).subscribe({
      next: data => {
        
        this.storageService.saveUser(data);
       
        this.isLoginFailed = false;
        this.storageService.setValueLoggedIn(true);
        this.storageService.setValueIsAdmin(this.storageService.isAdminFunc());
        if (this.router.url.includes("/needlogin") && this.LastPageThis !== null) {
          this.router.navigate([this.LastPageThis])
        } else if (this.router.url.includes("/needlogin")) {
          this.router.navigate(['/'])
        } else {
          window.location.reload()
        }
        this.storageService.setValueLastPage(null);
      },
      error: err => {
        alert("Could not log in");
        this.errorMessage = err.error.message;
        this.isLoginFailed = true;
      }
    });
  }

  logOut(): void {
    this.storageService.clean();
    this.storageService.setValueLoggedIn(false);
    window.location.reload();
  }

  reloadPage(): void {
    window.location.reload();
  }

}
