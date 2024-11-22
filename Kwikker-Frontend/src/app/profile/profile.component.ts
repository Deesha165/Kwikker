import { Component, ElementRef, ViewChild, AfterViewInit, Input } from '@angular/core';
import { TimelineService } from '../../Services/Timeline.service';
import { CreatedTweet } from '../../Models/Tweet.model';
import { CommonModule } from '@angular/common';
import { TweetComponent } from '../tweet/tweet.component';
import { BookmarkService } from '../../Services/Bookmark.service';
import { FollowService } from '../../Services/Follow.service';
import { CreatedUser, CustomUser, UserForUpdate } from '../../Models/User.model';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../Services/User.service';
import { TweetService } from '../../Services/Tweet.service';
import { TweetPostComponent } from '../tweet-post/tweet-post.component';
import { RequestParameters } from '../../Models/RequestParameters.model';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { AuthenticationService } from '../../Services/Authentication.service';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, TweetComponent, TweetPostComponent, InfiniteScrollModule,FormsModule],
  templateUrl:'./profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements AfterViewInit {
  requestParameters: RequestParameters = {
    PageNumber: 1,
    PageSize: 6,
    OrderBy: '',
    Fields: ''
  };

  appUserId:number=0;
  userId: number = 0;
  userForUpdate: UserForUpdate = {
    Id: 0,
    UserName: '',
    Bio: '',
    CoverPicture: '',
    Profilepicture: ''
  };

  followers: CustomUser[] = [];
  followees: CustomUser[] = [];
  user!: CreatedUser;
  count: number = 0;
  showTweetPost: boolean = false;
  tweetForUpdate!: CreatedTweet;

  allPostsIds=new Set<number>();
  bookmarkesIds = new Set<number>();
  likedTweetsIds = new Set<number>();
  userRetweetsIds = new Set<number>();

 allPosts=new Set<CreatedTweet>();
  bookmarkes:CreatedTweet[]=[];
  likedTweets :CreatedTweet[]=[];
  userRetweets:CreatedTweet[]=[];

  @ViewChild('posts', { static: false }) posts!: ElementRef<HTMLButtonElement>;
  @ViewChild('likes', { static: false }) likes!: ElementRef<HTMLButtonElement>;
  @ViewChild('bookmarks', { static: false }) bookmarks!: ElementRef<HTMLButtonElement>;
  @ViewChild('retweets', { static: false }) retweets!: ElementRef<HTMLButtonElement>;

  activeButton!: ElementRef<HTMLButtonElement>;

  isLoading: boolean = false;

  constructor(
  
    private timelineService: TimelineService,
    private bookmarksService: BookmarkService,
    private followService: FollowService,
    private userService: UserService,
    private tweetService: TweetService,
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthenticationService
  ) 
  {
    const storedUserId = localStorage.getItem('userId');

    // Check if 'userId' exists and is a valid number
    if (storedUserId) {
      this.appUserId = parseInt(storedUserId, 10); // parseInt with base 10
      console.log('followerId from suggested to follow',this.appUserId);
    }

  }

  ngOnInit(): void {
    this.route.paramMap.subscribe((paramMap) => {
      this.userId = +paramMap.get('id')!;
      console.log(this.userId);
      this.fetchProfileData();
    });
  }

  ngAfterViewInit(): void {
    this.activeButton = this.posts;
  
  }

  toggleLoading = () => (this.isLoading = !this.isLoading);

  onScroll = () => {
    if (this.activeButton == this.posts) this.getUserProfile();
  };

  fetchProfileData(): void {
   
    this.requestParameters.PageNumber=1;
    this.allPostsIds.clear();
    this.allPosts.clear();
    this.getUser();
    this.getFollowers();
    this.getFollowees();
    
    this.getUserBookmarks();
    this.getUserLikedTweets();
    this.getUserRetweets();
    this.getUserProfile();
  }

  getUser(): void {
    this.authService
      .handleUnauthorized(() => this.userService.getUserDynamic1(this.userId))
      .subscribe({
        next: (data) => (this.user = data),
        error: (error) => console.error('Error fetching user:', error)
      });
  }

  getUserProfile(): void {
    
    this.changeTapStatus(this.posts);
    this.toggleLoading();
    this.authService
      .handleUnauthorized(() =>
        this.timelineService.getProfile(this.userId, this.requestParameters)
      )
      .subscribe({
        next: (data) => {
          
          
          data.tweets.forEach((tweet) => {
            if(!this.allPostsIds.has(tweet.id))
            {
               this.allPosts.add(tweet);    
              this.allPostsIds.add(tweet.id);
            }
            
          });
          this.count = data.totalCount;
          this.requestParameters.PageNumber++;
        },
        error: (error) => console.error('Error fetching profile tweets:', error),
        complete: () => this.toggleLoading()
      });
  }

  getUserBookmarks(): void {
    this.changeTapStatus(this.bookmarks);
    this.authService
      .handleUnauthorized(() => this.bookmarksService.getBookmarks(this.userId))
      .subscribe({
        next: (data) => {
          this.bookmarkesIds.clear();
          this.bookmarkes=data||[];
          data.forEach((tweet) => {
          
            if(!this.bookmarkesIds.has(tweet.id))
            {
             
              this.bookmarkesIds.add(tweet.id);
            }
            
          });
          this.count = this.bookmarkesIds.size;
        },
        error: (error) => console.error('Error fetching bookmarks:', error)
      });
  }

  getUserLikedTweets(): void {
    this.changeTapStatus(this.likes);
    this.authService
      .handleUnauthorized(() =>
        this.timelineService.getUserLikedTweets(this.userId)
      )
      .subscribe({
        next: (data) => {
          this.likedTweets=data||[];
          this.likedTweetsIds.clear();
          data.forEach((tweet) =>{
            if(!this.likedTweetsIds.has(tweet.id))
            {
             
              this.likedTweetsIds.add(tweet.id);
            }
            
          } );
          this.count = this.likedTweetsIds.size;
        },
        error: (error) => console.error('Error fetching liked tweets:', error)
      });
  }

  getUserRetweets(): void {
    this.changeTapStatus(this.retweets);
    this.authService
      .handleUnauthorized(() =>
        this.timelineService.getUserRetweets(this.userId)
      )
      .subscribe({
        next: (data) => {
          this.userRetweets=data||[];
          this.userRetweetsIds.clear();
          data.forEach((tweet) => {
            if(!this.userRetweetsIds.has(tweet.id))
            {
             
              this.userRetweetsIds.add(tweet.id);
            }
          
          });
          this.count = this.userRetweetsIds.size;
        },
        error: (error) => console.error('Error fetching retweets:', error)
      });
  }

  getFollowers(): void {
    this.authService
      .handleUnauthorized(() => this.followService.getFollowers(this.userId))
      .subscribe({
        next: (data) => (this.followers = data || []),
        error: (error) => console.error('Error fetching followers:', error)
      });
  }

  getFollowees(): void {
    this.authService
      .handleUnauthorized(() => this.followService.getFollowees(this.userId))
      .subscribe({
        next: (data) => (this.followees = data || []),
        error: (error) => console.error('Error fetching followees:', error)
      });
  }

  changeTapStatus(selectedButton: ElementRef<HTMLButtonElement>): void {
    if (this.activeButton) {
      this.activeButton.nativeElement.style.backgroundColor = 'inherit';
      this.activeButton.nativeElement.style.color = 'rgb(119, 111, 111)';
    }
    selectedButton.nativeElement.style.backgroundColor = 'rgb(38, 39, 40)';
    selectedButton.nativeElement.style.color = 'white';
    this.activeButton = selectedButton;
  }

  goToFollowees(userId: number): void {
    this.router.navigate(['/followees', userId]);
  }

  goToFollowers(userId: number): void {
    this.router.navigate(['/followers', userId]);
  }

  handleDeletion(tweet: CreatedTweet): void {
    this.authService.handleUnauthorized(()=>this.tweetService.removeTweet(tweet.id)).subscribe();
    
  }

  handleUpdate(tweet: CreatedTweet): void {
    this.tweetForUpdate = tweet;
    this.showTweetPost = false;
    this.authService.handleUnauthorized(()=>this.tweetService.updateTweet(this.tweetForUpdate)).subscribe();
  }

  receiveForUpdate(tweet: CreatedTweet): void {
    this.showTweetPost = true;
    this.tweetForUpdate = tweet;
  }

  closePopUp(): void {
    this.showTweetPost = false;
  }

  ///updating user logic
  onSaveChanges()
  {
    console.log(this.user.Id,this.user.UserName,this.user.Bio)
          this.userForUpdate.Id=this.user.Id;
          this.userForUpdate.UserName=this.user.UserName;
          this.userForUpdate.Bio=this.user.Bio;
          this.userForUpdate.CoverPicture=this.user.CoverPicture;
          this.userForUpdate.Profilepicture=this.user.ProfilePicture;

         this.userService.updateUser(this.userForUpdate).subscribe();
  }
  onProfilePictureChange(event: Event): void {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = () => {
        this.user.ProfilePicture = reader.result;
      };
      reader.readAsDataURL(file);
    }
  }
  
  onCoverPhotoChange(event: Event): void {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = () => {
        this.user.CoverPicture = reader.result;
      };
      reader.readAsDataURL(file);
    }
  }


isPosts():boolean{
  return this.activeButton==this.posts;
}
isLikes():boolean{
  return this.activeButton==this.likes;
}
isRetweets():boolean{
  return this.activeButton==this.retweets;
}
isbookmarkes():boolean{
  return this.activeButton==this.bookmarks;
}

getPosts():CreatedTweet[]{
if(this.isbookmarkes()) return this.bookmarkes;
else if(this.isLikes()) return this.likedTweets;
else if (this.isRetweets()) return this.userRetweets;

return [...this.allPosts];
}

}
