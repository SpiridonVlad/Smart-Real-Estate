// filepath: /c:/Users/andre/Documents/GitHub/Smart-Real-Estate-1/Smart-Real-Estate/real-estate-management/src/app/components/record-details/record-details.component.ts
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RecordService } from '../../services/record.service';
import { Record } from '../../models/record.model';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-record-details',
  imports: [CommonModule],
  templateUrl: './record-details.component.html',
  styleUrls: ['./record-details.component.css']
})
export class RecordDetailsComponent implements OnInit {
  loading: boolean = true;
  defaultRecord = {
    address: {
      id: '',
      city: '',
      street: '',
      state: '',
      country: '',
      postalCode: ''
    },
    property: {
      imageId: '',
      type: 0,
      features: {}
     },
    user: {
      username: '',
      verified: false,
      rating: 0,
      type: 0, 
    },
    listing: {
      description: '',
      price: 0,
      publicationDate: new Date(),
      features: {}
    }
  };
  record: Record = this.defaultRecord;
  recordId: string = '';

  constructor(private route: ActivatedRoute, private recordService: RecordService) {}

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
        this.record = this.defaultRecord;
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
}