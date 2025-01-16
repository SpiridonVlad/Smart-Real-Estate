// news.component.ts
import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-news',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="news-container">
      <h2>Real Estate News</h2>
      <div class="news-carousel">
        <div class="news-item" [class.fade]="isTransitioning">
          <div class="news-date">{{ currentNews.date | date:'fullDate' }}</div>
          <h3>{{ currentNews.title }}</h3>
          <p>{{ currentNews.excerpt }}</p>
          <button class="read-more">Read More</button>
        </div>
        <div class="carousel-controls">
          <button class="carousel-button" (click)="previousNews()">←</button>
          <div class="carousel-indicators">
            <div
              *ngFor="let _, let i = index of newsItems"
              class="indicator"
              [class.active]="i === currentIndex"
              (click)="goToNews(i)">
            </div>
          </div>
          <button class="carousel-button" (click)="nextNews()">→</button>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .news-container {
      width: 300px;
      background: #1e1e1e; /* Darker background to match your theme */
      border-radius: 8px;
      box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2);
      padding: 20px;
      margin-right: 20px;
      height: fit-content;
      margin-top: 87px; /* This will align it with your records section */
    }

    .news-container h2 {
      color: #ffffff; /* White text for dark theme */
      margin-bottom: 20px;
      padding-bottom: 10px;
      border-bottom: 2px solid #007bff;
      text-align: left;
    }

    .news-carousel {
      position: relative;
    }

    .news-item {
      padding: 20px;
      border-radius: 6px;
      background: #2d2d2d; /* Darker background for news items */
      min-height: 300px;
      display: flex;
      flex-direction: column;
      opacity: 1;
      transition: opacity 0.3s ease-in-out;
    }

    .news-item.fade {
      opacity: 0;
    }

    .news-date {
      color: #aaaaaa; /* Light grey for dark theme */
      font-size: 0.9em;
      margin-bottom: 12px;
    }

    .news-item h3 {
      color: #ffffff; /* White text for dark theme */
      margin: 0 0 15px 0;
      font-size: 1.3em;
      line-height: 1.4;
    }

    .news-item p {
      color: #cccccc; /* Light grey for better readability */
      margin: 0 0 20px 0;
      line-height: 1.6;
      flex-grow: 1;
    }

    .read-more {
      background: #007bff;
      color: white;
      border: none;
      padding: 10px 20px;
      border-radius: 4px;
      cursor: pointer;
      transition: background 0.2s;
      align-self: flex-start;
    }

    .read-more:hover {
      background: #0056b3;
    }

    .carousel-controls {
      display: flex;
      align-items: center;
      justify-content: space-between;
      margin-top: 15px;
      padding: 0 10px;
    }

    .carousel-button {
      background: transparent;
      border: none;
      color: #007bff;
      font-size: 1.5em;
      cursor: pointer;
      padding: 5px 10px;
      transition: color 0.2s;
    }

    .carousel-button:hover {
      color: #0056b3;
    }

    .carousel-indicators {
      display: flex;
      gap: 8px;
    }

    .indicator {
      width: 8px;
      height: 8px;
      border-radius: 50%;
      background: #555; /* Darker indicators for dark theme */
      cursor: pointer;
      transition: background-color 0.2s;
    }

    .indicator.active {
      background: #007bff;
    }
  `]
})
export class NewsComponent implements OnInit, OnDestroy {
  newsItems = [
    {
      date: new Date(),
      title: 'Housing Market Shows Unprecedented Growth',
      excerpt: 'Recent data indicates a significant 15% increase in property values across major metropolitan areas, marking the strongest growth in the past decade. Experts suggest this trend might continue as demand remains high.'
    },
    {
      date: new Date(Date.now() - 86400000),
      title: 'New Real Estate Tax Regulations Coming Soon',
      excerpt: 'Government announces comprehensive updates to property tax guidelines affecting both homeowners and investors. The new framework aims to promote sustainable development while ensuring market stability.'
    },
    {
      date: new Date(Date.now() - 172800000),
      title: 'Sustainable Housing Becomes Top Priority',
      excerpt: 'Green building practices are becoming increasingly popular among new developments, with over 60% of new projects incorporating eco-friendly features. This shift reflects growing environmental awareness.'
    },
    {
      date: new Date(Date.now() - 259200000),
      title: 'Interest Rates Create New Opportunities',
      excerpt: 'Recent changes in lending rates have opened up new possibilities for first-time homebuyers. Financial experts recommend taking advantage of these favorable conditions while they last.'
    }
  ];

  currentIndex = 0;
  currentNews = this.newsItems[0];
  isTransitioning = false;
  private autoRotateInterval: any;

  ngOnInit() {
    this.startAutoRotate();
  }

  ngOnDestroy() {
    this.stopAutoRotate();
  }

  private startAutoRotate() {
    this.autoRotateInterval = setInterval(() => {
      this.nextNews();
    }, 5000); // Rotate every 5 seconds
  }

  private stopAutoRotate() {
    if (this.autoRotateInterval) {
      clearInterval(this.autoRotateInterval);
    }
  }

  nextNews() {
    this.goToNews((this.currentIndex + 1) % this.newsItems.length);
  }

  previousNews() {
    this.goToNews(this.currentIndex === 0 ? this.newsItems.length - 1 : this.currentIndex - 1);
  }

  goToNews(index: number) {
    this.isTransitioning = true;
    setTimeout(() => {
      this.currentIndex = index;
      this.currentNews = this.newsItems[index];
      this.isTransitioning = false;
    }, 300); // Match the CSS transition duration

    // Reset the auto-rotate timer
    this.stopAutoRotate();
    this.startAutoRotate();
  }
}
