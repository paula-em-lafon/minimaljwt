import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router'
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http'
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CookieService } from 'ngx-cookie-service';

import { AppComponent } from './app.component';
import { HeaderComponent } from './components/header/header.component';
import { MoviesComponent } from './components/movies/movies.component';
import { MovieItemComponent } from './components/movie-item/movie-item.component';
import { MoviePageComponent } from './components/movie-page/movie-page.component';

import appRoutes from './routerConfig';
import { LoginComponent } from './components/login/login.component';
import { NeedloginComponent } from './components/needlogin/needlogin.component';
import { JwtHelperService, JWT_OPTIONS } from '@auth0/angular-jwt';
import { CookieModule } from 'ngx-cookie';
import { EditCreateFormComponent } from './components/edit-create-form/edit-create-form.component';
import { CommonModule } from '@angular/common';
import { NeedadminComponent } from './components/needadmin/needadmin.component';
import { UserEditCreateComponent } from './components/user-edit-create/user-edit-create.component';
import { TokenInterceptor } from './RefreshInterceptor';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    MoviesComponent,
    MovieItemComponent,
    MoviePageComponent,
    LoginComponent,
    NeedloginComponent,
    EditCreateFormComponent,
    NeedadminComponent,
    UserEditCreateComponent
  ],
  imports: [
    BrowserModule,
    FontAwesomeModule,
    RouterModule.forRoot(appRoutes),
    HttpClientModule,
    FormsModule,
    CookieModule.forRoot(),
    CommonModule,
    ReactiveFormsModule
  ],
  providers: [CookieService,
    { provide: JWT_OPTIONS, useValue: JWT_OPTIONS },
    JwtHelperService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true,
    },],
  bootstrap: [AppComponent]
})
export class AppModule { }
