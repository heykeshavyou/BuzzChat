import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { initializeApp } from '@angular/fire/app';
import { getMessaging, getToken } from '@angular/fire/messaging';
import { Router, RouterOutlet } from '@angular/router';
import { firebaseConfig } from '../Environments/Environment';
import { UserService } from './Services/User/user-service';

@Component({
  selector: 'app-root',
  imports: [CommonModule, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App implements OnInit {
  private messaging: any;
  constructor(private _userService:UserService) {}
  ngOnInit(): void {
    const app = initializeApp(firebaseConfig);
    this.messaging = getMessaging(app);
    this.requestPermission();
  }
  requestPermission() {
    Notification.requestPermission().then((permission) => {
      if (permission == 'granted') {
        getToken(this.messaging, {
          vapidKey:firebaseConfig.vapidKey
        }).then((currentToken:string)=>{
          this._userService.Token=currentToken;
        })
      }
    });
  }
}
