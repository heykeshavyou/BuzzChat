import { Component } from '@angular/core';
import { ChatService } from '../../Services/Chat/chat-service';

@Component({
  selector: 'app-create-group',
  imports: [],
  templateUrl: './create-group.html',
  styleUrl: './create-group.css',
})
export class CreateGroup {
  constructor(public ChatService:ChatService){

  }
}
