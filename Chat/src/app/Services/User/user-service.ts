import { Injectable } from '@angular/core';
import User from '../../Models/User';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  user:User|null=null;
  SaveUser(newUser:User){
    this.user=newUser;
    let jsonString=JSON.stringify(this.user);
    localStorage.setItem("chatUser",jsonString);
  }
  GetUser(){
    let data=localStorage.getItem("chatUser")??"";
    let user :User = JSON.parse(data);
    this.user=user;
  }
}
