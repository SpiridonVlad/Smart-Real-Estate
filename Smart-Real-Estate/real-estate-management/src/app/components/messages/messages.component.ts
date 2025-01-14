import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MessageService } from '../../services/message.service';
import { AuthService } from '../../services/auth.service';
import { UserService } from '../../services/user.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HeaderComponent } from "../header/header.component";
import { FooterComponent } from "../footer/footer.component";

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css'],
  imports: [CommonModule, FormsModule, HeaderComponent, FooterComponent]
})
export class MessagesComponent implements OnInit {
  messages: any[] = [];
  currentUserId: string = '';
  usernames: { [key: string]: string } = {};
  chatId: string = ''; // Add chatId property

  constructor(
    private route: ActivatedRoute,
    private messageService: MessageService,
    private authService: AuthService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    const userId = this.authService.getUserId();
    if (userId) { // Ensure userId is not null
      this.currentUserId = userId;
    }
    const chatId = this.route.snapshot.paramMap.get('id');
    if (chatId) { // Ensure chatId is not null
      this.chatId = chatId; // Set chatId
      this.loadMessages(chatId);
    }
  }

  loadMessages(chatId: string): void {
    this.messageService.getMessages(chatId).subscribe({
      next: (response) => {
        this.messages = response.sort((a: any, b: any) => new Date(a.timestamp).getTime() - new Date(b.timestamp).getTime());
        this.loadUsernames();
      },
      error: (error) => console.error('Error fetching messages:', error),
    });
  }

  loadUsernames(): void {
    const senderIds = [...new Set(this.messages.map(message => message.senderId))];
    senderIds.forEach(senderId => {
      if (senderId !== this.currentUserId) {
        this.userService.getUserById(senderId).subscribe({
          next: (response) => {
            this.usernames[senderId] = response.data.username;
          },
          error: (error) => console.error('Error fetching user details:', error),
        });
      }
    });
  }

  getUsername(senderId: string): string {
    return this.usernames[senderId] || 'Unknown';
  }

  sendMessage(event: KeyboardEvent): void {
    if (event.key === 'Enter' && !event.shiftKey) { // Check if the Enter key was pressed without the Shift key
      const input = event.target as HTMLTextAreaElement;
      const content = input.value.trim();
      if (content) {
        this.messageService.sendMessage(this.chatId, { content }).subscribe({
          next: (response) => {
            input.value = ''; // Clear the input
            this.loadMessages(this.chatId); // Reload messages
          },
          error: (error) => console.error('Error sending message:', error),
        });
      }
      event.preventDefault(); // Prevent the default behavior of the Enter key
    }
  }
}
