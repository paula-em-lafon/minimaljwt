import { Routes, CanActivate } from '@angular/router'

import { MoviesComponent } from './components/movies/movies.component';
import { MoviePageComponent } from './components/movie-page/movie-page.component';
import { NeedloginComponent } from './components/needlogin/needlogin.component';
import { AuthguardService as AuthGuard } from './services/authguard.service'
import { EditCreateFormComponent } from './components/edit-create-form/edit-create-form.component';
import { UserEditCreateComponent } from './components/user-edit-create/user-edit-create.component'
import { AdminguardService } from './services/adminguard.service';
import { NeedadminComponent } from './components/needadmin/needadmin.component';

const appRoutes: Routes = [
  {
    path: '',
    component: MoviesComponent,
    pathMatch: 'full'
  },
  {
    path: 'needlogin',
    component: NeedloginComponent,
    pathMatch: 'full'
  },
  {
    path: 'needadmin',
    component: NeedadminComponent,
    pathMatch: 'full'
  },
  {
    path: 'movie/edit/:id',
    component: EditCreateFormComponent,
    canActivate: [AuthGuard, AdminguardService],
    pathMatch: 'full'
  },
  {
    path: 'movie/create',
    component: EditCreateFormComponent,
    canActivate: [AuthGuard, AdminguardService],
    pathMatch: 'full'
  },
  {
    path: 'movie/:id',
    component: MoviePageComponent,
    canActivate: [AuthGuard],
    pathMatch: 'full'
  },
  {
    path: 'user',
    component: UserEditCreateComponent,
    pathMatch: 'full'
  },
  {
    path: 'userchange',
    canActivate: [AuthGuard],
    component: UserEditCreateComponent,
    pathMatch: 'full'
  }


];
export default appRoutes;
