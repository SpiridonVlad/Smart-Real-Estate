import { Component, OnInit } from '@angular/core';
import { RecordService } from '../../services/record.service';
import { Record } from '../../models/record.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-record-list',
  imports: [CommonModule, FormsModule],
  templateUrl: './record-list.component.html',
  styleUrls: ['./record-list.component.css'],
})
export class RecordListComponent implements OnInit {
  records: Record[] = [];
  page: number = 1;
  pageSize: number = 5;
  pages: number[] = [1, 2, 3, 4, 5];
  pageSizes: number[] = [5, 10, 15];

  constructor(private recordService: RecordService) {}

  ngOnInit(): void {
    this.loadRecords();
  }

  loadRecords(): void {
    this.recordService.getPaginatedRecords(this.page, this.pageSize).subscribe(
      (response: any) => {
        this.records = response.data;
      },
      (error) => {
        console.error('Error loading records:', error);
      }
    );
  }

  getFeatures(features: { [key: string]: number }): { key: string; value: number }[] {
    return Object.entries(features).map(([key, value]) => ({ key, value }));
  }
}
