import { Component, ElementRef, HostListener, ViewChild } from '@angular/core';
import { TweetComponent } from "../tweet/tweet.component";
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CreatedTweet, TweetForCreation } from '../../Models/Tweet.model';
import { TweetService } from '../../Services/Tweet.service';
import { TimelineService } from '../../Services/Timeline.service';
import { TweetPostComponent } from "../tweet-post/tweet-post.component";
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { AuthenticationService } from '../../Services/Authentication.service';
import { UserService } from '../../Services/User.service';
import { CreatedUser } from '../../Models/User.model';
@Component({
  selector: 'app-home-timeline',
  standalone: true,
  imports: [TweetComponent, FormsModule, CommonModule, TweetPostComponent,InfiniteScrollModule],
  templateUrl: './home-timeline.component.html',
  styleUrl: './home-timeline.component.css'
})
export class HomeTimelineComponent {
 
 
  tweetToCreate: TweetForCreation = {
    content: '',
    mediaUrl: null
  };
   showTweetPost:boolean=false;
  isTweetValid: boolean = false;
  user!:CreatedUser;
  tweets=new Set<CreatedTweet>();

  tweetForUpdate!:CreatedTweet;
  activeButton!:ElementRef<HTMLButtonElement>;
  userId:number=0;
  // Check the tweet content whenever the input changes
  checkTweetContent(): void {
    this.isTweetValid = this.tweetToCreate.content.trim().length > 0;
  }

  @ViewChild('fileInput') fileInput!: ElementRef;
  @ViewChild('Foryou') ForYou!:ElementRef<HTMLButtonElement>; 
 @ViewChild('Following') Following!:ElementRef<HTMLButtonElement>;


 constructor(private tweetService:TweetService,private timelinService:TimelineService
  ,private authService: AuthenticationService,private userService:UserService)
 {
       
  const storedUserId = localStorage.getItem('userId');

  // Check if 'userId' exists and is a valid number
  if (storedUserId) {
    this.userId = parseInt(storedUserId, 10); // parseInt with base 10
  }

 }
 
  ngAfterViewInit():void{
    this.activeButton=this.ForYou;

    this.GetRandomTimeline();
    this.changeTapStatus(this.ForYou);
     this.getUser();
  }

 getUser()
 {
  this.authService.handleUnauthorized(()=>this.userService.getUserDynamic3(this.userId)).subscribe((data)=>{
    this.user=data;
  });
 }
 
 GetFollowersNews():void{
   
   if(this.activeButton==this.ForYou) {
    this.tweets.clear();
    this.changeTapStatus(this.Following); 
  }
    this.toggleLoading();
    this.authService.handleUnauthorized(()=>this.timelinService.getFollowersNews(this.userId))
    .subscribe((data:CreatedTweet[])=>{
    
       data.forEach(tweet=>{
      
        this.tweets.add(tweet);
       })
          
    },  (error) => {
      console.error('Error fetching home timeline', error);  // Handle error
    },()=>{
      this.toggleLoading();
    })
      }

  GetRandomTimeline():void{
    if(this.activeButton==this.Following) {
      this.tweets.clear();
      this.changeTapStatus(this.ForYou); 
      console.log('unexpected result');
    }
 
   this.toggleLoading();

   this.authService.handleUnauthorized(()=> this.timelinService.getRandomTimeline(this.userId))
   .subscribe((data:CreatedTweet[])=>{
      
      data.forEach(tweet=>{
        this.tweets.add(tweet);
       })

    
    },(error)=>{
      console.error('Error fetching random timeline',error);
    },()=>{
      this.toggleLoading();
    })
  }


  submitTweet(): void {

    this.authService.handleUnauthorized(()=>this.tweetService.createTweet(this.userId,this.tweetToCreate))
    .subscribe({
      next: (response) => {
        console.log('Response from server:', response,response.id);
        // Here you can access and handle the response from the backend
           if(response)
           {
           
           let newTweet:CreatedTweet={
             id: response.id,
             mediaUrl: response.mediaUrl,
             content: response.content,
             userId: response.userId,
             createdAt: response.createdAt,
             profilePicture: response.profilePicture,
             userName: response.userName,
             likesNumber:response.likesNumber,
             retweetsNumber:response.retweetsNumber,
             bookmarksNumber:response.bookmarksNumber,
             email:response.email
           }    
           this.tweets.add(newTweet);          
            
            this.tweetToCreate={
              content:'',
              mediaUrl:null
            }
              
           }
      },
      error: (error) => {
        console.error('Error occurred:', error);
        // Handle the error response
      }
    });
  }



    // Trigger the hidden file input
    triggerFileInput(): void {
      this.fileInput.nativeElement.click();
    }
  
    // Handle file selection
    onFileSelected(event: any): void {
       const file = event.target.files[0];
       if (file) {
        const reader = new FileReader();
  
        // Read the file content as a Data URL
        reader.onload = (e: any) => {
          
          this.tweetToCreate.mediaUrl = e.target.result;
        };
  
        reader.readAsDataURL(file);  // Read the file as Data URL (for image preview)
  
      }
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
    handleDeletion(tweet:CreatedTweet)
 {
  this.authService.handleUnauthorized(()=>this.tweetService.removeTweet(tweet.id)).subscribe();
  this.tweets.delete(tweet);
 }
 receiveForUpdate(tweet:CreatedTweet)
 {
       this.showTweetPost=true;
    this.tweetForUpdate=tweet;
 }
    handleUpdate(tweet:CreatedTweet){
      this.tweetForUpdate=tweet;
      this.showTweetPost=false;


     this.authService.handleUnauthorized(()=>this.tweetService.updateTweet(this.tweetForUpdate)).subscribe();

    }
   closePopUp():void{
          this.showTweetPost=false;
   }

   
  //implement infinite scroll
isLoading:boolean=false;
toggleLoading = ()=>this.isLoading=!this.isLoading;


onScroll= ()=>{

  if(this.activeButton==this.ForYou)  this.GetRandomTimeline();
  else if(this.activeButton==this.Following) this.GetFollowersNews();

}



  }

