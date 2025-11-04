import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { UserService } from '../User/user-service';
import { UserHub } from '../../Models/UserHub';
import { ApiService } from '../Api/api-service';
import { Router } from '@angular/router';


@Injectable({
  providedIn: 'root',
})
export class ChatService {
  private _hubConnection: signalR.HubConnection|null=null;
  private baseUrl="http://192.168.1.35:5500/";
  Users:UserHub[]=[];
  CurrentUser:UserHub|null=null;
  constructor(private _userService:UserService,private _router:Router){
    
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

  SetUser(id:Number){
    let index = this.Users.findIndex(x=>x.id==id);
    if(index!=-1){
      this.CurrentUser=this.Users[index];
      if(window.innerWidth<1280){
        this._router.navigate(['/chat']);
      }
    }
  }
}
