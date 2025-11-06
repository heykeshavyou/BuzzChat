import { Routes } from '@angular/router';
import { LoginApp } from './Authentication/login/login';
import { Signup } from './Authentication/signup/signup';
import { Home } from './Pages/home/home';
import { Chatlist } from './Pages/chatlist/chatlist';
import { Profile } from './Pages/profile/profile';
import { Chat } from './Pages/chat/chat';
import { AllUsers } from './Pages/all-users/all-users';

export const routes: Routes = [
    {path:"",component:Home ,children:[
        {path :'',component:Chatlist},
        {path :'profile',component:Profile},
        {path:'chat',component:Chat},
        {path:'all',component:AllUsers}
    ]},
    {path:"login",component:LoginApp},
    {path:"signup",component:Signup}
];
    