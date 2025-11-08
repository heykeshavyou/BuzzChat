import { ChangeDetectorRef, Component, OnInit, signal } from '@angular/core';
import { Loading } from '../../Components/Loading';
import { ChatService } from '../../Services/Chat/chat-service';
import { ApiService } from '../../Services/Api/api-service';
import { UserService } from '../../Services/User/user-service';

@Component({
  selector: 'app-all-users',
  imports: [Loading],
  templateUrl: './all-users.html',
  styleUrl: './all-users.css',
})
export class AllUsers implements OnInit {
  Loading = signal(false);
  constructor(
    public ChatService: ChatService,
    private _apiService: ApiService,
    private _userService: UserService,
    private _cdr: ChangeDetectorRef
  ) {}
  ngOnInit(): void {
    if (this.ChatService.Users.length == 0) {
      this.GetUsers();
    }
    this.ChatService.messagesChanged$.subscribe(() => {
      this._cdr.detectChanges();
    });
  }
  GetUsers() {
    this.Loading.set(true);
    this._apiService.GetUsers(this._userService.user?.token ?? '').subscribe(
      (res) => {
        this.ChatService.Users = res;
      },
      (error) => {
        console.log(error);
      },
      () => {
        this.Loading.set(false);
      }
    );
  }
}
