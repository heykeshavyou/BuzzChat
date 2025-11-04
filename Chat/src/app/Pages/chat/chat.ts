import { Component } from '@angular/core';
import { ChatService } from '../../Services/Chat/chat-service';

@Component({
  selector: 'app-chat',
  imports: [],
  templateUrl: './chat.html',
  styleUrl: './chat.css',
})
export class Chat {
  constructor(public ChatService:ChatService){

  }
}
