import { Component, OnInit } from '@angular/core';
import { UserService } from '../../Services/User/user-service';
import { Router } from '@angular/router';
import { ChatService } from '../../Services/Chat/chat-service';

@Component({
  selector: 'app-home',
  imports: [],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  constructor(private _userService: UserService, private _router: Router,private _chatService:ChatService) {

  }
  ngOnInit(): void {
    if (this._userService.user==null) {
      this._router.navigate(["/login"]);
    }else{
      this._chatService.Start();
    }
  }
}
