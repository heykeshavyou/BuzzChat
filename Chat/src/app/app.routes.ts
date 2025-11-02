import { Routes } from '@angular/router';
import { LoginApp } from './Authentication/login/login';
import { Signup } from './Authentication/signup/signup';

export const routes: Routes = [
    {path:"login",component:LoginApp,title:"Chat"},
    {path:"signup",component:Signup,title:"Chat"}
];
