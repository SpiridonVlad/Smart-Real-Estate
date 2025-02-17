// filepath: /c:/Users/andre/Documents/GitHub/Smart-Real-Estate-1/Smart-Real-Estate/real-estate-management/src/app/components/record-details/record-details.component.ts
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RecordService } from '../../services/record.service';
import { Record } from '../../models/record.model';
import { CommonModule } from '@angular/common';
import { MessageService } from '../../services/message.service';


@Component({
  selector: 'app-record-details',
  imports: [CommonModule],
  templateUrl: './record-details.component.html',
  styleUrls: ['./record-details.component.css']
})
export class RecordDetailsComponent implements OnInit {
  loading: boolean = true;
  record: Record | null = null;
  recordId: string = '';

  constructor(private route: ActivatedRoute, private recordService: RecordService, private messageService: MessageService, private router: Router) {}

  ngOnInit(): void {
    this.recordId = this.route.snapshot.paramMap.get('id')!;
    this.loadRecordDetails(this.recordId);
  }

  loadRecordDetails(id: string): void {
    this.recordService.getRecordById(id).subscribe(
      (response: any) => {
        const data = response.data; // Accesează proprietatea `data` din răspunsul primit
        this.record = data; // Actualizează `record` cu datele din `response.data`
        this.loading = false;
      },
      (error) => {
        this.record = null;
        this.loading = false;
        console.error('Error loading record:', error);
      }
    );
  }


  getFeatures(features: { [key: string]: number } | null | undefined): { key: string; value: number }[] {
    if (!features || typeof features !== 'object') {
      return [];
    }
    return Object.entries(features).map(([key, value]) => ({ key, value }));
  }

  getPropertyType(type: number): string {
    switch (type) {
      case 0:
        return 'Apartment';
      case 1:
        return 'Office';
      case 2:
        return 'Studio';
      case 3:
        return 'Commercial Space';
      case 4:
        return 'House';
      case 5:
        return 'Garage';
      default:
        return 'Unknown';
    }
  }

  createChat(userId: string): void {
    this.messageService.createChat(userId).subscribe({
      next: (response) => {
        const chatId = response.chatId;
        this.router.navigate([`/messages/${chatId}`]); // Navigate to messages/{chatId}
      },
      error: (error) => console.error('Error creating chat:', error),
    });
  }

}
