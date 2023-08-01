import { Routes, CanActivate } from '@angular/router'

import { MoviesComponent } from './components/movies/movies.component';
import { MoviePageComponent } from './components/movie-page/movie-page.component';
import { NeedloginComponent } from './components/needlogin/needlogin.component';
import { AuthguardService as AuthGuard } from './services/authguard.service'

const appRoutes: Routes = [
  {
    path: '',
    component: MoviesComponent,
    canActivate: [AuthGuard],
    pathMatch: 'full' 
  },
  {
    path: 'needlogin',
    component: NeedloginComponent,
    pathMatch: 'full'
  },
  {
    path: 'movie/:id',
    component: MoviePageComponent,
    pathMatch: 'full'
  }
];
export default appRoutes;
