import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import User from '../../Models/User';
import Login from '../../Models/Login';
import { SignUp } from '../../Models/SignUp';
import { UserHub } from '../../Models/UserHub';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  baseUrl="http://192.168.1.35:5500/api/";
  constructor(private _httpClient:HttpClient){

  }
  Login(model:Login):Observable<User>{
    return this._httpClient.post<User>(`${this.baseUrl}Account/Login`,model);
  }
  SignUp(model:SignUp):Observable<string>{
    return this._httpClient.post<string>(`${this.baseUrl}Account/Login`,model);
  }
  GetUsers(token:string):Observable<UserHub[]>{
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this._httpClient.get<UserHub[]>(`${this.baseUrl}Account/GetAllUsers`,{headers});
  }
}
