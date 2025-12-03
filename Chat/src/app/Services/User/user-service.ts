import { Injectable } from '@angular/core';
import User from '../../Models/User';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  user: User | null = null;
  Token:string="";
  constructor(private _router:Router) {
    this.GetUser();
  }
  SaveUser(newUser: User) {
    this.user = newUser;
    let jsonString = JSON.stringify(this.user);
    localStorage.setItem('chatUser', jsonString);
  }
  GetUser() {
    let data = localStorage.getItem('chatUser') ?? '';
    let user: User = data ? JSON.parse(data) : null;
    this.user = user;
  }
  Logout(){
    localStorage.removeItem('chatUser');
    this.user=null;
    this._router.navigate(['/login']);
  }
}
