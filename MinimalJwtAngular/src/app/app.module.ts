import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router'
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { HttpClientModule } from '@angular/common/http'
import { FormsModule } from '@angular/forms';
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

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    MoviesComponent,
    MovieItemComponent,
    MoviePageComponent,
    LoginComponent,
    NeedloginComponent
  ],
  imports: [
    BrowserModule,
    FontAwesomeModule,
    RouterModule.forRoot(appRoutes),
    HttpClientModule,
    FormsModule
  ],
  providers: [CookieService,
    { provide: JWT_OPTIONS, useValue: JWT_OPTIONS },
    JwtHelperService],
  bootstrap: [AppComponent]
})
export class AppModule { }
