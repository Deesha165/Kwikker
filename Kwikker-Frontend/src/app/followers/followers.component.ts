import { Component, ElementRef, Input, ViewChild } from '@angular/core';
import { CreatedUser, CustomUser } from '../../Models/User.model';
import { FollowService } from '../../Services/Follow.service';
import { CommonModule } from '@angular/common';
import { CreatedTweet } from '../../Models/Tweet.model';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from '../../Services/Authentication.service';
@Component({
  selector: 'app-followers',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './followers.component.html',
  styleUrl: './followers.component.css'
})
export class FollowersComponent {

  followers:CustomUser[]=[];

  followerId=0;
  
  followStates: { [userId: number]: boolean } = {};

activeButton!:ElementRef<HTMLButtonElement>;

  constructor (private followService:FollowService,private router:Router
    ,private route:ActivatedRoute,private authService:AuthenticationService)
    {
     
    }

  @ViewChild('followersbtn') followersbtn!:ElementRef<HTMLButtonElement>; 
  @ViewChild('followeesbtn') followeesbtn!:ElementRef<HTMLButtonElement>;
  ngOnInit():void{
    this.route.paramMap.subscribe(paramMap => {
      this.followerId = +paramMap.get('id')!; // Extract userId from route
        this.getFollowers();   
    });
  }




  ngAfterViewInit():void{
  
  this.activeButton=this.followersbtn;
 this.changeTapStatus(this.followersbtn);
}

getFollowers():void{
  
this.authService.handleUnauthorized(()=>this.followService.getFollowers(this.followerId))
.subscribe((data)=>{
   
    this.followers=data;
    console.log('followers',this.followers);
  },(error)=>{
   console.log('error at fetching followers data',(error));
  })
}




checkFollow(followeeId: number): void {

  if(this.followStates[followeeId])
    {

      this.authService.handleUnauthorized(()=>this.followService.removeFollow(this.followerId,followeeId))
      .subscribe((response)=>{
        console.log(response);
       })
    }
    else{
      this.authService.handleUnauthorized(()=>this.followService.createFollow(this.followerId,followeeId))
      .subscribe((response)=>{
        console.log(response);
      });
    }
  this.followStates[followeeId] = !this.followStates[followeeId]; // Toggle the follow state for the specific user
}

isFollowed(followeeId: number): boolean {
     
  return this.followStates[followeeId] || false; // Return follow state or false if not defined
}


changeTapStatus(selectedButton: ElementRef<HTMLButtonElement>): void {
  if (this.activeButton) {
    // Reset previous active button
    this.activeButton.nativeElement.style.backgroundColor = 'inherit';
    this.activeButton.nativeElement.style.color = 'rgb(119, 111, 111)';
  }
  

  // Set the new active button styles
  selectedButton.nativeElement.style.backgroundColor = 'rgb(38, 39, 40)';
  selectedButton.nativeElement.style.color = 'white';

  // Update the active button reference
  this.activeButton = selectedButton;
}
goToFollowers(): void {
  this.router.navigate(['followers',this.followerId]);
}

goToFollowees(): void {
  this.router.navigate(['followees',this.followerId]);
}


goToProfile(userId:number):void{

  this.router.navigate(['profile',userId]);
  }

}
