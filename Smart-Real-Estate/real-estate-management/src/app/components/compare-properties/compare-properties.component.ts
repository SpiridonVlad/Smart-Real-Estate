import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Record } from '../../models/record.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface FeatureComparison {
  featureType: number;
  baseValue: number;
  comparedValue: number;
  similarityScore: number;
}

interface AddressProximity {
  distanceInKilometers: number;
  similarityScore: number;
}

interface TypeCompatibility {
  baseType: number;
  comparedType: number;
  score: number;
}

interface ComparisonResult {
  overallSimilarityScore: number;
  featureComparisons: { [key: string]: FeatureComparison };
  addressProximity: AddressProximity;
  typeCompatibility: TypeCompatibility;
  winningProperty: string;
  reasons: string[];
}

@Component({
  selector: 'app-compare-properties',
  imports: [CommonModule, FormsModule],
  templateUrl: './compare-properties.component.html',
  styleUrls: ['./compare-properties.component.css']
})
export class ComparePropertiesComponent implements OnInit {
  properties: Record[] = [];
  comparisonResult: ComparisonResult | null = null;

  constructor(private route: ActivatedRoute, private http: HttpClient) {}

  ngOnInit(): void {
    // const initialId = this.route.snapshot.queryParamMap.get('Initial');
    // const secondaryId = this.route.snapshot.queryParamMap.get('Secondary');
    // if (initialId && secondaryId) {
    //   this.loadProperties(initialId, secondaryId);
    // } else {
      // Mock data for testing
      this.comparisonResult = {
        overallSimilarityScore: 0.85,
        featureComparisons: {
          Pool: { featureType: 2, baseValue: 0, comparedValue: 0, similarityScore: 1 },
          Year: { featureType: 7, baseValue: 1970, comparedValue: 1970, similarityScore: 1 },
          Alarm: { featureType: 16, baseValue: 0, comparedValue: 0, similarityScore: 1 },
          Attic: { featureType: 15, baseValue: 0, comparedValue: 0, similarityScore: 1 },
          Floor: { featureType: 6, baseValue: 1, comparedValue: 1, similarityScore: 1 },
          Rooms: { featureType: 4, baseValue: 3, comparedValue: 3, similarityScore: 1 },
          Garage: { featureType: 1, baseValue: 1, comparedValue: 1, similarityScore: 1 },
          Garden: { featureType: 0, baseValue: 0, comparedValue: 0, similarityScore: 1 },
          Balcony: { featureType: 3, baseValue: 0, comparedValue: 0, similarityScore: 1 },
          Parking: { featureType: 12, baseValue: 0, comparedValue: 0, similarityScore: 1 },
          Storage: { featureType: 13, baseValue: 0, comparedValue: 0, similarityScore: 1 },
          Surface: { featureType: 5, baseValue: 10, comparedValue: 10, similarityScore: 1 },
          Basement: { featureType: 14, baseValue: 0, comparedValue: 0, similarityScore: 1 },
          Elevator: { featureType: 10, baseValue: 0, comparedValue: 0, similarityScore: 1 },
          Intercom: { featureType: 17, baseValue: 0, comparedValue: 0, similarityScore: 1 },
          FireAlarm: { featureType: 19, baseValue: 1, comparedValue: 1, similarityScore: 1 },
          Furnished: { featureType: 11, baseValue: 0, comparedValue: 0, similarityScore: 1 },
          HeatingUnit: { featureType: 8, baseValue: 0, comparedValue: 0, similarityScore: 1 },
          AirConditioning: { featureType: 9, baseValue: 0, comparedValue: 0, similarityScore: 1 },
          VideoSurveillance: { featureType: 18, baseValue: 0, comparedValue: 0, similarityScore: 1 }
        },
        addressProximity: { distanceInKilometers: 5, similarityScore: 0.9 },
        typeCompatibility: { baseType: 0, comparedType: 0, score: 1 },
        winningProperty: 'e400e79f-67bd-416e-9623-927505a7df2f',
        reasons: ['Base property has better features overall.']
      };
    
  }

  // loadProperties(initialId: string, secondaryId: string): void {
  //   const url = `https://localhost:7117/api/Compare?Initial=${initialId}&Secondary=${secondaryId}`;
  //   this.http.get<{ data: ComparisonResult }>(url).subscribe(
  //     (response) => {
  //       this.comparisonResult = response.data;
  //     },
  //     (error: any) => {
  //       console.error('Error loading properties:', error);
  //     }
  //   );
  // }
}