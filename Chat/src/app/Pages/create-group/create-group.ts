import { Component, OnInit, signal } from '@angular/core';
import { ChatService } from '../../Services/Chat/chat-service';
import {
  FormArray,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { from } from 'rxjs';
import { ApiService } from '../../Services/Api/api-service';
import { UserService } from '../../Services/User/user-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-group',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './create-group.html',
  styleUrl: './create-group.css',
})
export class CreateGroup implements OnInit {
  Loading = signal(false);
  groupForm!: FormGroup;
  imagePreview: string | ArrayBuffer | null = null;
  constructor(public ChatService: ChatService, private _fb: FormBuilder,private _apiService:ApiService,private _userService:UserService,private _router:Router) {}
  ngOnInit(): void {
    this.groupForm = this._fb.group({
      Name: ['', Validators.required],
      Image: [null],
      Members: this._fb.array([], Validators.required),
    });
  }
  onCheckBoxClick(e: any) {
    const Members = this.groupForm.get('Members') as FormArray;
    if (e.target.checked) {
      Members.push(this._fb.control(e.target.value));
    } else {
      const index = Members.controls.findIndex(
        (x) => x.value === e.target.value
      );
      Members.removeAt(index);
    }
  }
  onFileSelected(event: any) {
    this.Loading.set(true);
    const file = event.target.files[0];
    this.groupForm.patchValue({ Image: file });

    let fileReader = new FileReader();

    fileReader.onload = () => {
      this.imagePreview = fileReader.result;
      this.Loading.set(false);
    };
    fileReader.readAsDataURL(file);
  }
  Create(e: Event) {
    e.preventDefault();
    if (this.groupForm.invalid) return;
    let form = new FormData();
    form.append('Name', this.groupForm.value.Name);
    if (this.groupForm.value.Image) {
      form.append('icon', this.groupForm.value.Image);
    }
    this.groupForm.value.Members.forEach((id:any)=>{
      form.append('users',id);
    });
    this._apiService.CreateGroup(form,this._userService.user?.token??'').subscribe(
      (res)=>{
        this.ChatService.Groups.push(res);
        this._router.navigate(['']);
      },(error)=>{
      }
    )
  }
}
