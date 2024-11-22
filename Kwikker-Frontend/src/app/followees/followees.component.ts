import { Component, ElementRef, ViewChild } from '@angular/core';
import { CreatedUser, CustomUser } from '../../Models/User.model';
import { FollowService } from '../../Services/Follow.service';
import { CommonModule } from '@angular/common';
import { Element } from '@angular/compiler';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from '../../Services/Authentication.service';
@Component({
  selector: 'app-followees',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './followees.component.html',
  styleUrl: './followees.component.css'
})
export class FolloweesComponent {

  userAppFollowees=new Set<number>();
  userAppId=0;// for authenticated user
  followerId=0;// for user profile
  followees:CustomUser[]=[];

activeButton!:ElementRef<HTMLButtonElement>;

  constructor (private followService:FollowService,private router:Router
    ,private route:ActivatedRoute,private authService:AuthenticationService)
    {
      const storedUserId = localStorage.getItem('userId');

      // Check if 'userId' exists and is a valid number
      if (storedUserId) {
        this.userAppId = parseInt(storedUserId, 10); // parseInt with base 10
      }
    }
  @ViewChild('followersbtn') followersbtn!:ElementRef<HTMLButtonElement>; 
  @ViewChild('followeesbtn') followeesbtn!:ElementRef<HTMLButtonElement>;
ngAfterViewInit():void{
  
  this.activeButton=this.followeesbtn;
 this.changeTapStatus(this.followeesbtn);
}

ngOnInit():void{
  this.route.paramMap.subscribe(paramMap => {
    this.followerId = +paramMap.get('id')!; // Extract userId from route
      this.getFollowees();   
  });
  this.getUserAppFollowees();
}

getFollowees():void{
  
this.authService.handleUnauthorized(()=>this.followService.getFollowees(this.followerId))
.subscribe((data)=>{
  
    this.followees=data;
    console.log('followees',this.followees);
  },(error)=>{
   console.log('error at fetching followees data',(error));
  })
}

getUserAppFollowees():void{
  
  this.authService.handleUnauthorized(()=>this.followService.getFollowees(this.userAppId))
  .subscribe((data)=>{
    this.userAppFollowees.clear();
    data.forEach(user=>{
      this.userAppFollowees.add(user.id);
    })
    
    },(error)=>{
     console.log('error at fetching followees data',(error));
    })
  }


checkFollow(followeeId: number): void {

  if(this.isFollowed(followeeId))
    {

      this.authService.handleUnauthorized(()=>this.followService.removeFollow(this.userAppId,followeeId))
      .subscribe((response)=>{
        console.log(response);
        this.userAppFollowees.delete(followeeId);
       })
    
    }
    else{

     this.authService.handleUnauthorized(()=> this.followService.createFollow(this.userAppId,followeeId))
     .subscribe((response)=>{
        console.log(response);
        this.userAppFollowees.add(followeeId);
      });
    }
  
}

isFollowed(followeeId: number): boolean {

  return this.userAppFollowees.has(followeeId);
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

  this.router.navigate(['/profile',userId]);
  }
}