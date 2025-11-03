import { Injectable } from '@angular/core';
import User from '../../Models/User';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  user: User | null = null;
  constructor() {
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
}
