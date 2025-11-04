import { Component, OnInit, signal } from '@angular/core';
import { ChatService } from '../../Services/Chat/chat-service';
import { ApiService } from '../../Services/Api/api-service';
import { UserService } from '../../Services/User/user-service';
import { Loading } from '../../Components/Loading';

@Component({
  selector: 'app-chatlist',
  imports: [Loading],
  templateUrl: './chatlist.html',
  styleUrl: './chatlist.css',
})
export class Chatlist implements OnInit {
  Loading = signal(true);
  constructor(
    public ChatService: ChatService,
    private _apiService: ApiService,
    private _userService: UserService
  ) {}
  ngOnInit(): void {
    this.GetUsers();
  }
  GetUsers() {
    this.Loading.set(true);
    this._apiService.GetUsers(this._userService.user?.token ?? '').subscribe(
      (res) => {
        this.ChatService.Users = res;
          },
      (error) => {},
      () => {
        this.Loading.set(false);
      }
    );
  }
}
