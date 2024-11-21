import { Component, ElementRef, Input, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NotificationService } from '../../Services/Notification.service';
import { AuthenticationService } from '../../Services/Authentication.service';
import { CreatedUser } from '../../Models/User.model';
import { UserService } from '../../Services/User.service';
import { CreatedTweet } from '../../Models/Tweet.model';
import { FormsModule } from '@angular/forms';
import { TweetPostComponent } from "../tweet-post/tweet-post.component";
import { TweetService } from '../../Services/Tweet.service';
@Component({
  selector: 'app-side-bar',
  standalone: true,
  imports: [CommonModule, FormsModule, TweetPostComponent],
  templateUrl: './side-bar.component.html',
  styleUrl: './side-bar.component.css'
})
export class SideBarComponent {

  

userId:number=0;

user!:CreatedUser;

notificationCount!: number;
constructor(private router:Router,private notificationService: NotificationService
  ,private authService:AuthenticationService, private userService:UserService,private tweetService:TweetService)
  {
 const storedUserId = localStorage.getItem('userId');

  // Check if 'userId' exists and is a valid number
  if (storedUserId) {
    this.userId = parseInt(storedUserId, 10); // parseInt with base 10
    console.log('from side bar',this.userId);
  }
}

ngOnInit():void{
  this.notificationService.notificationCount$.subscribe((data)=>{
    this.notificationCount=data;
  })
  this.getUser();

}


 // Reset notification count when viewing notifications
 viewNotifications() {
  this.notificationService.resetNotificationCount();

  this.router.navigate(['notifications',this.userId]);

}

getUser()
{

this.authService.handleUnauthorized(()=>this.userService.getUserDynamic2(this.userId)).subscribe((data)=>{
  this.user=data;
});
}

goToProfile(userId:number):void{

  this.router.navigate(['profile',userId]);
  }

  logOut(){
    this.authService.logout();
    
  }

  isTweetValid: boolean = false; 
  tweet: CreatedTweet ={
    id: 0,
    mediaUrl: null,
    content: '',
    userId: 0,
    createdAt: new Date(),
    profilePicture: null,
    userName: '',
    email: '',
    likesNumber: 0,
    retweetsNumber: 0,
    bookmarksNumber: 0
  }
  @ViewChild('fileInput') fileInput!: ElementRef;
  
// Check if the tweet content is valid (non-empty)
checkTweetContent(): void {
  this.isTweetValid = this.tweet.content.trim().length > 0;
}

// Save the tweet (either create or update)
saveTweet(): void {
  
    this.authService.handleUnauthorized(() => this.tweetService.createTweet(this.userId, this.tweet)).subscribe(() => {
       
})

}

// Trigger the hidden file input for uploading a profile picture
triggerFileInput(): void {
  this.fileInput.nativeElement.click();
}

// Handle file selection and preview the selected image
onFileSelected(event: any): void {
  const file = event.target.files[0];
  if (file) {
    const reader = new FileReader();
    reader.onload = (e: any) => {
      this.tweet.mediaUrl = e.target.result; // Assign the image URL to the tweet's mediaUrl
    };
    reader.readAsDataURL(file); // Read the file as Data URL for preview
  }
}
}
