import { Component, HostListener, OnInit } from '@angular/core';
import { UserService } from '../../Services/User/user-service';
import { Router, RouterOutlet, RouterLinkWithHref } from '@angular/router';
import { ChatService } from '../../Services/Chat/chat-service';
import { Chat } from '../chat/chat';

@Component({
  selector: 'app-home',
  imports: [RouterOutlet, RouterLinkWithHref, Chat],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  initialInnerWidth: number;
  constructor(
    private _userService: UserService,
    private _router: Router,
    public _chatService: ChatService
  ) {
    this.initialInnerWidth = window.innerWidth;
  }
  ngOnInit(): void {
    if (this._userService.user == null) {
      this._router.navigate(['/login']);
    } else {
      this._chatService.Start();
      this._chatService.MessageRecieve();
      this._chatService.GetOnlineUsers();
      this._chatService.UserDisconnected();
      this._chatService.UserConnected();
      this._chatService.NewGroupCreated();
    }
  }
  @HostListener('window:resize', ['$event'])
  onResize(event: Event) {
    let a = (event.target as Window).innerWidth;
    this.initialInnerWidth = a;
  }
  @HostListener('document:keydown.escape', ['$event'])
  onEscape(event: Event) {
    this._chatService.CurrentGroup = null;
  }
}
