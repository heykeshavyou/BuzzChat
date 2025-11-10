import { Injectable, NgZone } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { UserService } from '../User/user-service';
import { UserHub } from '../../Models/UserHub';
import { Router } from '@angular/router';
import Group from '../../Models/Group';
import Message from '../../Models/Message';
import { ApiService } from '../Api/api-service';
import { Subject } from 'rxjs';
import User from '../../Models/User';

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  private _hubConnection: signalR.HubConnection | null = null;
  private isLocal = true;
  private baseUrl = this.isLocal
    ? 'https://localhost:7059/'
    : 'http://192.168.1.35:5500/';
  Users: UserHub[] = [];
  Groups: Group[] = [];
  CurrentGroup: Group | null = null;
  public messagesChanged$ = new Subject<void>();
  ActiveUsers: UserHub[] = [];

  constructor(
    private _userService: UserService,
    private _router: Router,
    private _apiService: ApiService,
    private zone: NgZone
  ) {}
  public Start = () => {
    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.baseUrl}Buzz/TalkHub`, {
        accessTokenFactory: () => {
          return this._userService.user?.token ?? '';
        },
      })
      .withAutomaticReconnect()
      .build();
    let model = {
      Id: this._userService.user?.id,
      Name: this._userService.user?.name,
      Email: this._userService.user?.email,
      Username: this._userService.user?.username,
    };
    this._hubConnection
      .start()
      .then(() => {
        this._hubConnection?.invoke('ConnectUser', model);
        this._hubConnection
          ?.invoke('GetGroups')
          .then((res) => {
            this.zone.run(() => {
              this.Groups = res;
              this.messagesChanged$.next();
            });
          })
          .catch((error) => {});
      })
      .catch((error) => {
        console.log(error);
      });
  };
  SetUser(id: Number) {
    let index = this.Groups.findIndex((x) => x.id == id);
    if (index != -1) {
      this.CurrentGroup = this.Groups[index];
      if (window.innerWidth < 1280) {
        this._router.navigate(['/chat']);
      }
    }
  }

  CreateNewChat(id: number) {
    let group = this.FindUserGroup(id);
    if (group == null) {
      let user = this.Users.find((x) => x.id == id);
      if (user != undefined) {
        let group1: Group = {
          users: [],
        };
        group1.users?.push(user);
        this.CurrentGroup = group1;
      }
    } else {
      this.CurrentGroup = group;
    }
    if (window.innerWidth < 1280) {
      this._router.navigate(['/chat']);
    }
  }
  SendMessage(message: Message) {
    this._hubConnection
      ?.invoke('SendMessage', message)
      .then((res) => {
        this.zone.run(() => {
          if (message.groupId == null) {
            this.GetGroupById(res.groupId);
          }
          let index = this.Groups.findIndex((x) => x.id == res.groupId);
          this.Groups[index].messages?.push(res);
          this.CurrentGroup = this.Groups[index];
          this.messagesChanged$.next();
        });
      })
      .catch((err) => {
        console.log(err);
      });
  }
  GetCurrentGroupUser(): UserHub | undefined {
    const users = this.CurrentGroup?.users;
    const filtered = users?.filter((x) => x.id != this._userService.user?.id);
    return filtered?.[0];
  }
  GetChatName(id: number): string {
    let group = this.Groups.find((x) => x.id == id);
    if (group?.users?.length == 2) {
      let user = group.users.filter((x) => x.id != this._userService.user?.id);
      if (user) {
        return user[0].name;
      }
    }
    return '';
  }
  FindUserGroup(id: number): Group | null {
    let group = this.Groups.find((x) => x.users?.find((y) => y.id == id));
    let users = group?.users?.filter((x) => x.id != this._userService.user?.id);
    if (users?.length == 1) {
      return group ?? null;
    }
    return null;
  }
  GetGroupById(id: number) {
    this._apiService
      .GetGroupById(this._userService.user?.token ?? '', id)
      .subscribe(
        (res) => {
          this.Groups.push(res);
          this.CurrentGroup = res;
        },
        (err) => {}
      );
  }
  MessageRecieve() {
    this._hubConnection?.on('NewMessageReceive', (message: Message) => {
      this.zone.run(() => {
        let index = this.Groups.findIndex((x) => x.id == message.groupId);

        if (index != -1) {
          if (
            this.Groups[index].messages?.findIndex((x) => x.id == message.id) ==
            -1
          ) {
            this.Groups[index].messages?.push(message);
            if (
              this.CurrentGroup?.id == message.groupId &&
              this.CurrentGroup != null
            ) {
              this.CurrentGroup = this.Groups[index];
            }
          }
        }
        this.messagesChanged$.next();
      });
    });
  }
  GetOnlineUsers() {
    this._hubConnection?.on('ConnectedUsers', (users: UserHub[]) => {
      this.zone.run(() => {
        this.ActiveUsers = users;
        this.messagesChanged$.next();
      });
    });
  }
  UserDisconnected() {
    this._hubConnection?.on('UserDisconnected', (user: UserHub) => {
      this.zone.run(() => {
        let index = this.ActiveUsers.findIndex((x) => x.id == user.id);
        if (index != -1) {
          this.ActiveUsers.splice(index, 1);
          this.messagesChanged$.next();
        }
      });
    });
  }
  UserConnected() {
    this._hubConnection?.on('UserConnected', (user: UserHub) => {
      this.zone.run(() => {
        this.ActiveUsers.push(user);
        this.messagesChanged$.next();
      });
    });
  }
  NewGroupCreated() {
    this._hubConnection?.on('NewGroupCreated', (group: Group) => {
      this.zone.run(() => {
        group.messages = [];
        this.Groups.push(group);
        this.messagesChanged$.next();
      });
    });
  }
  NewUserSignUp() {
    this._hubConnection?.on('NewUserSignIn', (user: UserHub) => {
      this.zone.run(() => {
        this.Users.push(user);
        this.messagesChanged$.next();
      });
    });
  }
  GetActiveUser(id: number) {
    let user = this.GetGroupUser(id);
    if (user != null) {
      let index = this.ActiveUsers.findIndex((x) => x.id == user.id);
      return index == -1 ? false : true;
    }
    return false;
  }
  GetGroupUser(id: number): UserHub | null {
    let group = this.Groups.find((x) => x.id == id);
    if (group?.users?.length == 2) {
      let user = group.users.filter((x) => x.id != this._userService.user?.id);
      return user[0] ?? null;
    }
    return null;
  }
}
