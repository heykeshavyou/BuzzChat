import {
  AfterViewChecked,
  ChangeDetectorRef,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
  viewChild,
} from '@angular/core';
import { ChatService } from '../../Services/Chat/chat-service';
import { UserHub } from '../../Models/UserHub';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import Message from '../../Models/Message';
import { UserService } from '../../Services/User/user-service';
import User from '../../Models/User';

@Component({
  selector: 'app-chat',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './chat.html',
  styleUrl: './chat.css',
})
export class Chat implements OnInit, AfterViewChecked {
  @ViewChild('chatContainer') private chatContainer!: ElementRef;
  Message = new FormControl('', [Validators.required]);
  User: User | null = null;
  constructor(
    public ChatService: ChatService,
    private _userService: UserService,
    private _cdr: ChangeDetectorRef
  ) {}
  ngOnInit(): void {
    this.User = this._userService.user;
    this.ChatService.messagesChanged$.subscribe(() => {
      this._cdr.detectChanges();
      this.scrollToBottom();
    });
  }
  ngAfterViewChecked(): void {
    this.scrollToBottom();
  }
  GetChatName(): string {
    let user = this.ChatService.GetCurrentGroupUser();
    if (user) {
      return user?.name;
    }
    return '';
  }
  GetChatId(): number | null {
    let user = this.ChatService.GetCurrentGroupUser();
    if (user) {
      return user?.id;
    }
    return null;
  }

  SendMessage(e: Event) {
    e.preventDefault();
    if (this.Message.value == null || this.Message.value == '') return;
    let message: Message = {
      content: this.Message.value,
      fromId: this._userService.user?.id,
      toId:
        this.ChatService.CurrentGroup?.name == null ? this.GetChatId() : null,
      groupId: this.ChatService.CurrentGroup?.id,
    };
    var a = this.ChatService.SendMessage(message);
    this.Message.setValue('');
    this.scrollToBottom();
    this._cdr.detectChanges();
  }
  scrollToBottom(): void {
    try {
      this.chatContainer.nativeElement.scrollTop =
        this.chatContainer.nativeElement.scrollHeight;
    } catch (err) {}
  }
}
