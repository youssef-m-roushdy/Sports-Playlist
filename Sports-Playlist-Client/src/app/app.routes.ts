// app.routes.ts
import { Routes } from '@angular/router';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { HomeComponent } from './components/home/home.component';
import { AuthGuard, NoAuthGuard } from './guards/auth.guard';
import { WatchComponent } from './components/watch/watch.component';
import { MatchFormComponent } from './components/match-form/match-form.component';
import { WelcomeComponent } from './components/welcome/welcome.component';

export const routes: Routes = [
  { 
    path: 'welcome', 
    component: WelcomeComponent,
    title: 'Welcome to Sports Playlist'
  },
  { 
    path: 'login', 
    component: LoginComponent,
    title: 'Login',
    canActivate: [NoAuthGuard] 
  },
  { 
    path: 'register', 
    component: RegisterComponent,
    title: 'Register',
    canActivate: [NoAuthGuard]
  },
  { 
    path: 'home', 
    component: HomeComponent,
    title: 'Home',
    canActivate: [AuthGuard]
  },
  { 
    path: 'watch/:id', 
    component: WatchComponent,
    canActivate: [AuthGuard]
  },
  { 
    path: 'match/new', 
    component: MatchFormComponent,
    canActivate: [AuthGuard],
    title: 'Create Match'
  },
  { 
    path: 'match/edit/:id', 
    component: MatchFormComponent,
    canActivate: [AuthGuard],
    title: 'Edit Match'
  },
  { 
    path: '', 
    redirectTo: 'welcome', 
    pathMatch: 'full'
  },
  { 
    path: '**', 
    redirectTo: 'welcome' 
  }
];