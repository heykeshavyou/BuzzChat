import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { UserService } from '../User/user-service';


@Injectable({
  providedIn: 'root',
})
export class ChatService {
  private _hubConnection: signalR.HubConnection|null=null;
  private baseUrl="http://192.168.1.35:5500/";
  constructor(private _userService:UserService){
    
  }
  public Start=()=>{
    this._hubConnection=new signalR.HubConnectionBuilder()
    .withUrl(`${this.baseUrl}Buzz/TalkHub`,{
      accessTokenFactory:()=>{
        return this._userService.user?.token??'';
      }
    })
    .build();
    let model={
      Id:this._userService.user?.id,
      Name:this._userService.user?.name,
      Email:this._userService.user?.email,
      Username:this._userService.user?.username,
    }
    this._hubConnection.start().then(()=>{
     this._hubConnection?.invoke('ConnectUser',model);
    }).catch((error)=>{
      console.log(error);
    })
  }
}
