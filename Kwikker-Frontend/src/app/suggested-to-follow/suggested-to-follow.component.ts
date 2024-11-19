import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreatedUser } from '../../Models/User.model';
import { FollowService } from '../../Services/Follow.service';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthenticationService } from '../../Services/Authentication.service';
@Component({
  selector: 'app-suggested-to-follow',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './suggested-to-follow.component.html',
  styleUrl: './suggested-to-follow.component.css'
})
export class SuggestedToFollowComponent {
  
  suggestedUsers :CreatedUser[]=[];

  followerId:number=0;
  followStates: { [userId: number]: boolean } = {};
 
constructor (private followService:FollowService,private router:Router,private authService :AuthenticationService)
{
  const storedUserId = localStorage.getItem('userId');

  // Check if 'userId' exists and is a valid number
  if (storedUserId) {
    this.followerId = parseInt(storedUserId, 10); // parseInt with base 10
    console.log('followerId from suggested to follow',this.followerId);
  }
}


getSuggestedFollowers(): void {

  this.authService.handleUnauthorized(()=>this.followService.getSuggestedUsersToFollow(this.followerId))
  .subscribe(
    (data: CreatedUser[]) => {
      this.suggestedUsers = data;
      console.log('process done successfuly');
    },
    (error:HttpErrorResponse) => {
      console.error('Error fetching suggested users', error.status);
    }
  );
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
      this.authService.handleUnauthorized(()=> this.followService.createFollow(this.followerId,followeeId))
     .subscribe((response)=>{
        console.log(response);
      });
    }
  this.followStates[followeeId] = !this.followStates[followeeId]; // Toggle the follow state for the specific user
}

isFollowed(followeeId: number): boolean {
     
  return this.followStates[followeeId] || false; // Return follow state or false if not defined
}

goToProfile(userId:number):void{

this.router.navigate(['profile',userId]);
}

}
