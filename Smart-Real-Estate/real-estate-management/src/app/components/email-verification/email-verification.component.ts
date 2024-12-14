import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-email-verification',
  templateUrl: './email-verification.component.html',
  styleUrls: ['./email-verification.component.css']
})
export class EmailVerificationComponent implements OnInit {
  token: string | null = null;
  message: string = '';

  constructor(private route: ActivatedRoute, private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.token = this.route.snapshot.queryParamMap.get('token');
    if (this.token) {
      this.verifyEmail(this.token);
    } else {
      this.message = 'Invalid verification link.';
    }
  }

  verifyEmail(token: string): void {
    this.authService.verifyEmail(token).subscribe(
      response => {
        this.message = 'Email verified successfully!';
        // Optionally, navigate to another page
        // this.router.navigate(['/login']);
      },
      error => {
        console.error('Error verifying email:', error);
        this.message = 'Error verifying email. Please try again later.';
      }
    );
  }
}