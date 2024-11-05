import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
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
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule,RouterOutlet, SideBarComponent, LogInComponent, TrendsComponent, SuggestedToFollowComponent, TweetComponent, HomeTimelineComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Kwikker-Frontend';
  userId=2;
  users:CreatedUser[]=[];
  trends:CreatedTrend[]=[];

  constructor (private followService:FollowService,private trendService:TrendService)
  {

  }

  ngOnInit():void{
    console.log("test from app");
    this.getSuggestedFollowers();
    this.getRecentTrends();
  }

  getSuggestedFollowers():void
  {
    this.followService.getSuggestedUsersToFollow(this.userId).subscribe((data:CreatedUser[])=>{
      this.users=data;
   
    },(error)=>{
      console.error('Error fetching suggested users',error);
    })
  }

  getRecentTrends():void{
    this.trendService.getTrends().subscribe((data)=>{
    this.trends=data;

    console.log(this.trends);
    })
   }
    
  }
