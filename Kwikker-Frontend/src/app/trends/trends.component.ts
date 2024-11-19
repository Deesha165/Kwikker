import { Component, Input } from '@angular/core';

import { CreatedTrend } from '../../Models/Trend.model';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { TrendService } from '../../Services/Trend.service';
@Component({
  selector: 'app-trends',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './trends.component.html',
  styleUrl: './trends.component.css'
})
export class TrendsComponent {
 
  trends!:CreatedTrend[];
  @Input()
  userId=0;
  constructor(private router:Router,   private trendService: TrendService){}


  ngOnInit():void{
    this.getRecentTrends();
  } 
  getRecentTrends(): void {
    this.trendService.getTrends().subscribe((data) => {
      this.trends = data;
      console.log(this.trends);
    });
  }
   goToTrending(hashtag:string)
   {
    console.log('test',hashtag,this.userId);
         this.router.navigate(['trends',hashtag,this.userId]);
         
   }



}
