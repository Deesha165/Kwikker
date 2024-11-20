import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, NavigationEnd, RouterOutlet } from '@angular/router';
import { SideBarComponent } from "./side-bar/side-bar.component";
import { LogInComponent } from "./log-in/log-in.component";
import { TrendsComponent } from "./trends/trends.component";
import { SuggestedToFollowComponent } from "./suggested-to-follow/suggested-to-follow.component";
import { TweetComponent } from "./tweet/tweet.component";
import { HomeTimelineComponent } from "./home-timeline/home-timeline.component";
import { CreatedUser } from '../Models/User.model';
import { FollowService } from '../Services/Follow.service';
import { TrendService } from '../Services/Trend.service';
import { CreatedTrend } from '../Models/Trend.model';
import { CommonModule } from '@angular/common';
import { AuthenticationService } from '../Services/Authentication.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, SideBarComponent, LogInComponent, TrendsComponent, SuggestedToFollowComponent, TweetComponent, HomeTimelineComponent],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Kwikker-Frontend';
  userId = 2;

  routePart: string = '';

  constructor(   private router: Router,private authService :AuthenticationService) 
  {}

  ngOnInit(): void {
    console.log("test from app");

    // Listen to router events to capture route changes
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        const currentUrl = this.router.url.split('/');  // Split the URL by '/'
        this.routePart = currentUrl[1] || '';  // Get the first path segment (e.g., "signup")
        console.log("Current route part:", this.routePart);
      }
    });

   
  }

  isAuthenticated():boolean
  {
    return this.authService.isAuthenticated();
  }

}
