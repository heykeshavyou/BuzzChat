import { AfterViewChecked, ChangeDetectorRef, Component, OnInit, signal } from '@angular/core';
import { ChatService } from '../../Services/Chat/chat-service';
import { ApiService } from '../../Services/Api/api-service';
import { UserService } from '../../Services/User/user-service';
import { Loading } from '../../Components/Loading';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-chatlist',
  imports: [Loading, RouterLink,CommonModule],
  templateUrl: './chatlist.html',
  styleUrl: './chatlist.css',
})
export class Chatlist implements OnInit {
  Loading = signal(true);
  constructor(
    public ChatService: ChatService,
    private _apiService: ApiService,
    private _userService: UserService,
    private _cdr:ChangeDetectorRef
  ) {}
  ngOnInit(): void {
    if (this.ChatService.Groups.length == 0) {
      this.GetGroups();
    } else {
      this.Loading.set(false);
    }
        this.ChatService.messagesChanged$.subscribe(() => {
      this._cdr.detectChanges();
    });
  }
  GetGroups() {
    this.Loading.set(true);
    this._apiService.GetGroups(this._userService.user?.token ?? '').subscribe(
      (res) => {
        this.ChatService.Groups = res;
      },
      (error) => {},
      () => {
        this.Loading.set(false);
      }
    );
  }
  GetChatName(id: number) {
    return this.ChatService.GetChatName(id);
  }
}
