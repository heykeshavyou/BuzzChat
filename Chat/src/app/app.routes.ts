import { Routes } from '@angular/router';
import { LoginApp } from './Authentication/login/login';
import { Signup } from './Authentication/signup/signup';
import { Home } from './Pages/home/home';

export const routes: Routes = [
    {path:"",component:Home},
    {path:"login",component:LoginApp},
    {path:"signup",component:Signup}
];
