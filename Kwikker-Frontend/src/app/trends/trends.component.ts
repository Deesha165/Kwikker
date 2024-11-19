import { Component, Input, OnInit } from '@angular/core';
import { CreatedTrend } from '../../Models/Trend.model';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { TrendService } from '../../Services/Trend.service';
import { AuthenticationService } from '../../Services/Authentication.service';

@Component({
  selector: 'app-trends',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './trends.component.html',
  styleUrls: ['./trends.component.css']
})
export class TrendsComponent implements OnInit {
  trends: CreatedTrend[] = [];
 userId = 0;

  constructor(
    private router: Router,
    private trendService: TrendService,
    private authService: AuthenticationService
  ) {
    const storedUserId = localStorage.getItem('userId');

    // Check if 'userId' exists and is a valid number
    if (storedUserId) {
      this.userId = parseInt(storedUserId, 10); // parseInt with base 10

      console.log('from trends component',this.userId);
    }
  }

  ngOnInit(): void {
    this.loadRecentTrends();
  }

  loadRecentTrends(): void {
     this.trendService.getTrends()
      .subscribe({
        next: (data) => {
          this.trends = data;
          console.log('Loaded trends:', this.trends);
        },
        error: (error) => {
          console.error('Failed to load trends:', error);
        }
      });
  }

  goToTrending(hashtag: string): void {
    console.log('Navigating to trend:', hashtag, this.userId);
    this.router.navigate(['trends', hashtag, this.userId]);
  }
}
