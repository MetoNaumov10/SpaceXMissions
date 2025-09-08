import { Routes } from '@angular/router';
import { LoginComponent } from '../app/components/login/login.component';
import { MissionComponent } from '../app/components/missions/missions.component';
import { SignupComponent } from '../app/components/signup/signup.component';

export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'missions', component: MissionComponent },
    { path: 'signup', component: SignupComponent },
    { path: '**', redirectTo: 'login' }
];
