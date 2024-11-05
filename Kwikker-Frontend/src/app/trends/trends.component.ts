import { Component, Input } from '@angular/core';

import { CreatedTrend } from '../../Models/Trend.model';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
@Component({
  selector: 'app-trends',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './trends.component.html',
  styleUrl: './trends.component.css'
})
export class TrendsComponent {
 @Input()
  trends!:CreatedTrend[];
  @Input()
  userId=0;
  constructor(private router:Router){}

   

   goToTrending(hashtag:string)
   {
    console.log('test',hashtag,this.userId);
         this.router.navigate(['trends',hashtag,this.userId]);
         
   }



}
