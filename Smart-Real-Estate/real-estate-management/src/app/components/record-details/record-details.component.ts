import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HeaderComponent } from "../header/header.component";
import { FooterComponent } from "../footer/footer.component";

@Component({
  selector: 'app-record-details',
  templateUrl: './record-details.component.html',
  styleUrls: ['./record-details.component.css'],
  standalone: true,
  imports: [CommonModule, HeaderComponent, FooterComponent],
})
export class RecordDetailsComponent implements OnInit {
  record: any;
  recordSections: { title: string; items?: { label: string; value: any }[]; list?: { key: string; value: any }[] }[] = [];

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.loadRecord(id);
  }

  loadRecord(id: string | null): void {
    const mockData = this.getMockData();
    this.record = mockData.find((record) => record.listing.id === id);

    if (this.record) {
      this.prepareSections();
    }
  }

  prepareSections(): void {
    this.recordSections = [
      {
        title: "Property Info",
        items: this.getFeatures(this.record.property.features).filter(feature => feature.value !== 0),
      },
      {
        title: "Listing Info",
        items: [
          { label: "Price", value: `$${this.record.listing.price}` },
          { label: "Publication Date", value: new Date(this.record.listing.publicationDate).toLocaleDateString() },
          ...this.getListingFeatures(this.record.listing.features),
        ],
      },
      {
        title: "User Info",
        items: [
          { label: "Username", value: this.record.user.username },
          { label: "Verified", value: this.record.user.verified ? "Yes" : "No" },
          { label: "Rating", value: this.record.user.rating },
          { label: "Type", value: this.record.user.type },
        ],
      },
    ];
  }
  
  getFeatures(features: any): { label: string; value: any }[] {
    return Object.entries(features).map(([key, value]) => ({ label: key, value }));
  }
  
  getListingFeatures(features: any): { label: string; value: string }[] {
    const featureLabels: { [key: string]: string } = {
      IsSold: "Sold",
      IsHighlighted: "Highlighted",
      IsDeleted: "Deleted",
      ForSale: "For sale",
      ForRent: "For rent",
      ForLease: "For lease",
    };
  
    return Object.entries(features)
      .filter(([key, value]) => value === 1)
      .map(([key]) => ({ label: featureLabels[key], value: "" }));
  }

  getMockData(): any[] {
    return [
      {
        address: {
          street: "Main St",
          city: "Springfield",
          state: "IL",
          postalCode: "62704",
          country: "USA",
          additionalInfo: "Near the park",
        },
        property: {
          imageId: "img123",
          type: 1,
          features: {
            Garden: 1,
            Garage: 1,
            Pool: 0,
            Balcony: 1,
            Rooms: 5,
            Surface: 200,
            Floor: 2,
            Year: 1995,
            HeatingUnit: 1,
            AirConditioning: 1,
            Elevator: 0,
            Furnished: 1,
            Parking: 1,
            Storage: 1,
            Basement: 0,
            Attic: 0,
            Alarm: 1,
            Intercom: 1,
            VideoSurveillance: 0,
            FireAlarm: 1,
          },
        },
        user: {
          username: "john_doe",
          verified: true,
          rating: 4.5,
          type: 1,
        },
        listing: {
          id: "1",
          description: "A spacious 5-bedroom house near downtown Springfield.",
          price: 250000,
          publicationDate: "2025-01-11T10:00:00.000Z",
          features: {
            IsSold: 0,
            IsHighlighted: 1,
            IsDeleted: 0,
            ForSale: 1,
            ForRent: 0,
            ForLease: 0,
          },
        },
      },
      {
        address: {
          street: "Elm St",
          city: "Dallas",
          state: "TX",
          postalCode: "75201",
          country: "USA",
          additionalInfo: "Close to public transport",
        },
        property: {
          imageId: "img456",
          type: 2,
          features: {
            Garden: 0,
            Garage: 0,
            Pool: 1,
            Balcony: 1,
            Rooms: 3,
            Surface: 120,
            Floor: 10,
            Year: 2015,
            HeatingUnit: 1,
            AirConditioning: 1,
            Elevator: 1,
            Furnished: 1,
            Parking: 1,
            Storage: 0,
            Basement: 0,
            Attic: 0,
            Alarm: 0,
            Intercom: 1,
            VideoSurveillance: 1,
            FireAlarm: 1,
          },
        },
        user: {
          username: "jane_smith",
          verified: false,
          rating: 3.8,
          type: 0,
        },
        listing: {
          id: "2",
          description: "Modern apartment with stunning city views.",
          price: 180000,
          publicationDate: "2025-01-10T15:30:00.000Z",
          features: {
            IsSold: 0,
            IsHighlighted: 0,
            IsDeleted: 0,
            ForSale: 0,
            ForRent: 1,
            ForLease: 1,
          },
        },
      },
      {
        address: {
          street: "Oak Dr",
          city: "Seattle",
          state: "WA",
          postalCode: "98101",
          country: "USA",
          additionalInfo: "Quiet residential area",
        },
        property: {
          imageId: "img789",
          type: 3,
          features: {
            Garden: 1,
            Garage: 1,
            Pool: 0,
            Balcony: 0,
            Rooms: 4,
            Surface: 150,
            Floor: 1,
            Year: 2005,
            HeatingUnit: 1,
            AirConditioning: 0,
            Elevator: 0,
            Furnished: 0,
            Parking: 1,
            Storage: 1,
            Basement: 1,
            Attic: 1,
            Alarm: 1,
            Intercom: 0,
            VideoSurveillance: 1,
            FireAlarm: 1,
          },
        },
        user: {
          username: "michael_brown",
          verified: true,
          rating: 4.9,
          type: 2,
        },
        listing: {
          id: "3",
          description: "Charming family home with a large garden.",
          price: 320000,
          publicationDate: "2025-01-09T08:45:00.000Z",
          features: {
            IsSold: 0,
            IsHighlighted: 1,
            IsDeleted: 0,
            ForSale: 1,
            ForRent: 0,
            ForLease: 0,
          },
        },
      },
    ];
  }
}
