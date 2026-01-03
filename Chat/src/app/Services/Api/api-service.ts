import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import User from '../../Models/User';
import Login from '../../Models/Login';
import { SignUp } from '../../Models/SignUp';
import { UserHub } from '../../Models/UserHub';
import Group from '../../Models/Group';
import { UserService } from '../User/user-service';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private isLocal = false;
  private baseUrl = this.isLocal
    ? 'https://localhost:7059/api/'
    : 'http://192.168.1.2:45455/api/';
  constructor(private _httpClient: HttpClient) {}
  Login(model: Login): Observable<User> {
    return this._httpClient.post<User>(`${this.baseUrl}Account/Login`, model);
  }
  SignUp(model: SignUp): Observable<string> {
    return this._httpClient.post<string>(`${this.baseUrl}Account/Login`, model);
  }
  GetUsers(token: string): Observable<UserHub[]> {
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this._httpClient.get<UserHub[]>(
      `${this.baseUrl}Account/GetAllUsers`,
      { headers }
    );
  }
  GetGroups(token: string): Observable<Group[]> {
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this._httpClient.get<Group[]>(`${this.baseUrl}Group/GetAllGroups`, {
      headers,
    });
  }
  GetGroupById(token: string, id: number): Observable<Group> {
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this._httpClient.get<Group>(`${this.baseUrl}Group/group/${id}`, {
      headers,
    });
  }
  CreateGroup(model: any, token: string): Observable<Group> {
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this._httpClient.post<Group>(`${this.baseUrl}Group/Create`, model, {
      headers,
    });
  }
  ChangeName(name: string, token: string): Observable<string> {
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    return this._httpClient.post<string>(
      `${this.baseUrl}Account/ChangeName`,
      { name },   
      { headers }
    );
  }
}
