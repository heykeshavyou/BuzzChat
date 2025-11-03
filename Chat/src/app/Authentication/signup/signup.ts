import { Component, OnInit, signal } from '@angular/core';
import { UserService } from '../../Services/User/user-service';
import { Router, RouterLink } from '@angular/router';
import { ApiService } from '../../Services/Api/api-service';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { SignUp } from '../../Models/SignUp';
import { Loading } from '../../Components/Loading';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-signup',
  imports: [Loading,ReactiveFormsModule,CommonModule,RouterLink],
  templateUrl: './signup.html',
  styleUrl: './signup.css',
})
export class Signup implements OnInit {
  Username = new FormControl('', [Validators.required]);
  Password = new FormControl('', [Validators.required]);
  Email = new FormControl('', [Validators.required]);
  Name = new FormControl('', [Validators.required]);
  Loading = signal(false);
  constructor(
    private _userService: UserService,
    private _router: Router,
    private _apiService: ApiService
  ) {}
  ngOnInit(): void {
    if (this._userService.user != null) {
      this._router.navigate(['/']);
    }
  }
  SignUp(event: Event) {
    event.preventDefault();
    if (
      this.Username.value == null ||
      this.Password.value == null ||
      this.Name.value == null ||
      this.Email.value == null
    ) {
      return;
    }
    this.Loading.set(true);
    let model: SignUp = {
      username: this.Username.value,
      password: this.Password.value,
      name: this.Name.value,
      email: this.Email.value,
    };
    this._apiService.SignUp(model).subscribe(
      (res) => {
        this._router.navigate(['/login']);
      },
      (error) => {},
      () => {}
    );
  }
}
