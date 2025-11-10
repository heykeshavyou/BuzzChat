import {
  AfterViewChecked,
  ChangeDetectorRef,
  Component,
  OnInit,
  signal,
} from '@angular/core';
import { ChatService } from '../../Services/Chat/chat-service';
import { ApiService } from '../../Services/Api/api-service';
import { UserService } from '../../Services/User/user-service';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-chatlist',
  imports: [ RouterLink, CommonModule],
  templateUrl: './chatlist.html',
  styleUrl: './chatlist.css',
})
export class Chatlist implements OnInit {
  constructor(
    public ChatService: ChatService,
    private _apiService: ApiService,
    private _userService: UserService,
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
}
