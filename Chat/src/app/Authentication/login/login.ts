import { Component, model, OnInit, signal } from '@angular/core';
import {ReactiveFormsModule,FormControl,Validators} from '@angular/forms';
import { Loading } from '../../Components/Loading';
import { Router, RouterLink } from "@angular/router";
import Login from '../../Models/Login';
import { ApiService } from '../../Services/Api/api-service';
import { UserService } from '../../Services/User/user-service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, Loading, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class LoginApp implements OnInit {
  Username = new FormControl('', [Validators.required]);
  Password = new FormControl('', [Validators.required]);
  Loading=signal(false);
  constructor(private _apiService:ApiService,private _userService:UserService,private _router:Router) {

  }
  ngOnInit(): void {
    if(this._userService.user?.token!=""){
      this._router.navigate(["/"]);
    }else{
        this._router.navigate(["/login"]);
    }
  }
  Login(event:Event){
    event.preventDefault();
    if(this.Username.value==null||this.Password.value==null){
      return;
    }
    this.Loading.set(true);
    let model:Login ={
      username:this.Username.value,
      password:this.Password.value
    }
    this._apiService.Login(model).subscribe(
      (res)=>{
        console.log(res);
      },
      (error)=>{

      },
      ()=>{
        this.Loading.set(false);
      }
    );
  }
}
