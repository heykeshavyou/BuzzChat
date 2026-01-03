import { Component, signal } from '@angular/core';
import { UserService } from '../../Services/User/user-service';
import User from '../../Models/User';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../Services/Api/api-service';

@Component({
  selector: 'app-profile',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css',
})
export class Profile {
  User: User | null;
  Name = new FormControl('', [Validators.required]);
  EditName = signal(false);
  constructor(
    private _userService: UserService,
    private _apiService: ApiService
  ) {
    this.User = _userService.user;
    this.Name.setValue(this.User?.name ?? '');
  }
  SaveName() {
    if (
      this.Name.value == this.User?.name ||
      this.Name.value == null ||
      this.User?.token == undefined
    ) {
      this.EditName.set(false);
      return;
    }
    this._apiService.ChangeName(this.Name.value, this.User?.token).subscribe(
      (res) => {
        if (this.Name.value && this.User) {
          this.User.name = this.Name.value;
          this._userService.SaveUser(this.User);
          this.EditName.set(false);
        } 
      },
      (error) => {
        
      }
    );
  }
}
