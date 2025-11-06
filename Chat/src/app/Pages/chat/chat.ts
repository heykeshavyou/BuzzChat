import { Component } from '@angular/core';
import { ChatService } from '../../Services/Chat/chat-service';
import { UserHub } from '../../Models/UserHub';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import Message from '../../Models/Message';
import { UserService } from '../../Services/User/user-service';

@Component({
  selector: 'app-chat',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './chat.html',
  styleUrl: './chat.css',
})
export class Chat {
  Message = new FormControl('', [Validators.required]);
  constructor(public ChatService: ChatService,private _userService:UserService) {}
  GetChatName(): string {
    let user = this.ChatService.GetCurrentGroupUser();
    if (user) {
      return user?.name;
    }
    return '';
  }
  GetChatId():number|null{
     let user = this.ChatService.GetCurrentGroupUser();
    if (user) {
      return user?.id;
    }
    return null;
  }
  
  SendMessage(e: Event) {
    e.preventDefault();
    if (this.Message.value == null||this.Message.value=="") return;
    let message: Message = {
      content: this.Message.value,
      fromId:this._userService.user?.id,
      toId:(this.ChatService.CurrentGroup?.name==null)?this.GetChatId():null,
      groupId:this.ChatService.CurrentGroup?.id
    };
    this.ChatService.SendMessage(message);
    this.Message.setValue("");
  }
}
