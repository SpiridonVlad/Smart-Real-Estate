// home.component.ts
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { AiService } from '../../services/ai.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { HeaderComponent } from "../header/header.component";
import { FooterComponent } from "../footer/footer.component";
import { interval, Subscription } from 'rxjs';
import { CommonModule } from '@angular/common';

interface Property {
  title: string;
  location: string;
  price: string;
  features: string;
  description: string;
  imageId?: string;
}
interface PropertyArticle {
  tag: string;
  title: string;
  preview: string;
  date: string;
  readTime: string;
  type: string;
}
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  standalone: true,
  imports: [HeaderComponent, FooterComponent, CommonModule]
})
export class HomeComponent implements OnInit, OnDestroy {
  isLoggedIn: boolean = false;
  prediction: number | null = null;
  predictionForm: FormGroup;
  currentIndex: number = 0;
  private intervalSubscription?: Subscription;
  currentArticleIndex: number = 0;
  private articleIntervalSubscription?: Subscription;

  properties: Property[] = [
    {
      title: "Luxury Beachfront Villa",
      location: "Miami Beach, FL",
      price: "€2,450,000",
      features: "5 Beds • 4 Baths • 3,200 sq ft",
      description: "Contemporary villa with breathtaking ocean views and private beach access.",
      imageId: "https://propertyspark.com/wp-content/uploads/2017/05/20-Incredible-Houses-for-Sale-in-Miami-Florida-34-W-Dilido-Dr.jpg"
    },
    {
      title: "Modern Downtown Loft",
      location: "New York, NY",
      price: "€1,850,000",
      features: "2 Beds • 2 Baths • 1,800 sq ft",
      description: "Stunning industrial-style loft with floor-to-ceiling windows and city views.",
      imageId: "https://propertyspark.com/wp-content/uploads/2017/05/20-Incredible-Houses-for-Sale-in-Miami-Florida-2201-Collins-Ave-Phs-26-28.jpg"
    },
    {
      title: "Mountain View Estate",
      location: "Aspen, CO",
      price: "€3,950,000",
      features: "6 Beds • 5 Baths • 4,500 sq ft",
      description: "Luxurious mountain estate with panoramic views and ski-in/ski-out access.",
      imageId: "https://propertyspark.com/wp-content/uploads/2017/05/20-Incredible-Houses-for-Sale-in-Miami-Florida-2318-N-Bay-Rd.jpg"
    },
    {
      title: "Historic Brownstone",
      location: "Boston, MA",
      price: "€2,750,000",
      features: "4 Beds • 3.5 Baths • 3,000 sq ft",
      description: "Beautifully restored 19th century brownstone with original details.",
      imageId: "https://propertyspark.com/wp-content/uploads/2017/05/20-Incredible-Houses-for-Sale-in-Miami-Florida-2901-Collins-Ave-Ph-1602.jpg"
    },
    {
      title: "Waterfront Modern Home",
      location: "Seattle, WA",
      price: "€3,250,000",
      features: "5 Beds • 4 Baths • 3,800 sq ft",
      description: "Contemporary masterpiece with stunning water views and private dock.",
      imageId: "https://propertyspark.com/wp-content/uploads/2017/05/20-Incredible-Houses-for-Sale-in-Miami-Florida-321-Ocean-Dr-Phs-900-901.jpg"
    }
  ];


  // Add to your component class
  sidebarArticles: PropertyArticle[] = [
    {
      tag: "Market Analysis",
      title: "Luxury Real Estate Market Shows Strong Growth in Q1 2025",
      preview: "The luxury real estate sector has demonstrated remarkable resilience. Experts attribute this growth to rising demand from international investors and limited inventory in key markets.",
      date: "Jan 10, 2025",
      readTime: "5 min read",
      type: "Market Update"
    },
    {
      tag: "Investment Strategy",
      title: "Top 5 Emerging Markets for Real Estate",
      preview: "",
      date: "",
      readTime: "",
      type: "Investment Strategy"
    },
  ];


  constructor(
    private router: Router,
    private authService: AuthService,
    private aiService: AiService,
    private fb: FormBuilder,
    public dialog: MatDialog
  ) {
    this.predictionForm = this.fb.group({
      surface: [50, Validators.required],
      rooms: [2, Validators.required],
      description: ['string', Validators.required],
      price: [0, Validators.required],
      address: ['string', Validators.required],
      year: [2014, Validators.required],
      parking: [true, Validators.required],
      floor: [0, Validators.required]
    });
  }

  ngOnInit(): void {
    this.isLoggedIn = this.authService.isLoggedIn();
    this.startArticleRotation();
  }

  ngOnDestroy(): void {
    if (this.intervalSubscription) {
      this.intervalSubscription.unsubscribe();
    }
    if (this.articleIntervalSubscription) {
      this.articleIntervalSubscription.unsubscribe();
    }
  }

  startArticleRotation(): void {
    // Rotate articles every 7 seconds (slightly offset from property rotation)
    this.articleIntervalSubscription = interval(7000).subscribe(() => {
      this.currentArticleIndex = (this.currentArticleIndex + 1) % this.sidebarArticles.length;
    });
  }

  setCurrentArticle(index: number): void {
    this.currentArticleIndex = index;
    // Reset the interval when manually changing articles
    if (this.articleIntervalSubscription) {
      this.articleIntervalSubscription.unsubscribe();
    }
    this.startArticleRotation();
  }

  fetchPrediction(): void {
    const data = {
      surface: 50,
      rooms: 2,
      description: 'string',
      price: 0,
      address: 'string',
      year: 2014,
      parking: true,
      floor: 0
    };

    this.aiService.getPrediction(data).subscribe(
      (result) => {
        this.prediction = result;
        alert(`The predicted price is: ${this.prediction}€`);
      },
      (error) => {
        console.error('Error fetching prediction:', error);
      }
    );
  }

  navigateTo(route: string): void {
    if (this.authService.isLoggedIn()) {
      this.router.navigate([`/${route}`]);
    } else {
      this.router.navigate(['/login']);
    }
  }

  logout(): void {
    this.authService.logout();
    this.isLoggedIn = false;
    this.router.navigate(['']);
  }
}
