import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { AiService } from '../../services/ai.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  isLoggedIn: boolean = false;
  prediction: number | null = null;
  predictionForm: FormGroup;

  constructor(private router: Router,
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
  ngOnInit(): void {
    this.isLoggedIn = this.authService.isLoggedIn();
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