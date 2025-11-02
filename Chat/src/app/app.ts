import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { UserService } from './Services/User/user-service';

@Component({
  selector: 'app-root',
  imports: [CommonModule, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App  implements OnInit{
  constructor(private _userService:UserService,private _router:Router){

  }
  ngOnInit(): void {
    this._userService.GetUser();
  } 
}
