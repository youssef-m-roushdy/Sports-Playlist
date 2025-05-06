import { Routes } from '@angular/router';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { HomeComponent } from './components/home/home.component';
import { AuthGuard } from './guards/auth.guard';

export const routes: Routes = [
    { 
      path: 'login', 
      component: LoginComponent,
      title: 'Login' 
    },
    { 
      path: 'register', 
      component: RegisterComponent,
      title: 'Register' 
    },
    { 
      path: 'home', 
      component: HomeComponent,
      title: 'Home',
      canActivate: [AuthGuard]
    },
    { 
      path: '', 
      redirectTo: 'home', 
      pathMatch: 'full' 
    },
    { 
      path: '**', 
      redirectTo: 'home' 
    }
  ];