import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { StorageService } from '../../services/storage.service';
import { mustMatch } from '../../services/MustMatchValidator';
import { Location } from '@angular/common';

@Component({
  selector: 'app-user-edit-create',
  templateUrl: './user-edit-create.component.html',
  styleUrls: ['./user-edit-create.component.css']
})
export class UserEditCreateComponent {
  form: FormGroup;
  username: string;
  isLoggedIn: boolean;
  isAddMode: boolean;
  loading = false;
  submitted = false;
  toggleOverlay = false;
  deleted: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private userService: AuthService,
    private storageService: StorageService,
    private location: Location
  ) { }

  ngOnInit() {
    this.storageService.getValueLoggedIn().subscribe(value => {
      this.isLoggedIn = value;
    });

    if (this.isLoggedIn === true && this.location.path() === "/user") {
      this.router.navigate(['userchange']);
    }

    this.isAddMode = this.location.path() === "/user";

    if (!this.isAddMode) {
      this.username = this.storageService.getUser().username;
    }

    // password not required in edit mode
    const passwordValidators = [Validators.minLength(6)];
    if (this.isAddMode) {
      passwordValidators.push(Validators.required);
    }
    else {
      passwordValidators.push(Validators.nullValidator);
    }

    this.form = this.formBuilder.group({
      username: ['', Validators.required],
      givenName: ['', Validators.required],
      surname: ['', Validators.required],
      emailAddress: ['', [Validators.required, Validators.email]],
      role: ['', Validators.required],
      password: ['', passwordValidators],
      confirmPassword: ['', passwordValidators]
    }, {
      validator: mustMatch('password', 'confirmPassword')
    });

    if (!this.isAddMode) {
      this.userService.getUser()
        .subscribe(x => {
          this.form.controls.username.setValue(x.username)
          this.form.controls.givenName.setValue(x.givenName)
          this.form.controls.surname.setValue(x.surname)
          this.form.controls.emailAddress.setValue(x.emailAddress)
          this.form.controls.role.setValue(x.role)
          this.form.controls.password.setValue('');
          this.form.controls.confirmPassword.setValue('');
        });
    }
  }

  // convenience getter for easy access to form fields
  get f() { return this.form.controls; }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.form.invalid) {
      console.log(this.f)
      return;
    }

    this.loading = true;
    if (this.isAddMode) {
      this.createUser();
    }
    else {
      this.updateuser();
    }
  }

  private createUser() {
    this.userService.create(this.form.value)
      .subscribe({
        next: () => {
          alert('User added');
          this.router.navigate(['/']);
        },
        error: (error: any) => {
          alert(error.error);
          this.loading = false;
        }
      });
  }

  private updateuser() {
    this.userService.updateUser(this.form.value)
      .subscribe({
        next: () => {
          this.userService.tryRefreshingTokens()
            .then((success) => {
              if (success) {
                alert('User updated');
                this.router.navigate(['/']);
              }
              else {
                alert('Due to an error you have been logged out, try logging in with your new credentials');
                this.storageService.clean();
                this.storageService.setValueLoggedIn(false);
                this.router.navigate(['/']);
              }
            });
        },
        error: (error: any) => {
          alert(error.error);
          this.loading = false;
        }
      });
  }

  delete(): void {
    this.toggleOverlay = true;
    this.userService.deleteUser().subscribe({
      next: (value) => {
        this.deleted = value;
        this.finishDelete()
      },
      error: (error) => {
        this.finishDelete()
      }
    })
  }

  finishDelete(): void {
    this.toggleOverlay = false;
    if (this.deleted == true) {
      this.storageService.clean();
      this.storageService.setValueLoggedIn(false);
      this.router.navigate(['/'])
    }
    else {
      alert("The page could not be deleted")
    }
  }
}
