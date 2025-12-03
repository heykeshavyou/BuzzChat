import { ChangeDetectorRef, Component, OnInit, signal } from '@angular/core';
import { ChatService } from '../../Services/Chat/chat-service';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AllUsers } from '../all-users/all-users';
import { UserService } from '../../Services/User/user-service';

@Component({
  selector: 'app-chatlist',
  imports: [RouterLink, CommonModule, AllUsers],
  templateUrl: './chatlist.html',
  styleUrl: './chatlist.css',
})
export class Chatlist implements OnInit {
  MenuClass = 'hidden';
  IsMenuOpen= signal(false);
  Tab = signal(true);
  constructor(
    private _userService:UserService,
    public ChatService: ChatService,
    private _cdr: ChangeDetectorRef
  ) {}
  ngOnInit(): void {
    this.ChatService.messagesChanged$.subscribe(() => {
      this._cdr.detectChanges();
    });
  }
  GetChatName(id: number) {
    return this.ChatService.GetChatName(id);
  }
  OpenMenu(){
    if(this.IsMenuOpen()){
      this.MenuClass="hidden";
      this.IsMenuOpen.set(false);
    }else{
      this.MenuClass="block";
      this.IsMenuOpen.set(true);
    }
  }
  Logout(){
    this._userService.Logout();
  }
}
