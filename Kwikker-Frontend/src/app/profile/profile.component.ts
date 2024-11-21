import { Component, ElementRef, ViewChild, AfterViewInit, Input } from '@angular/core';
import { TimelineService } from '../../Services/Timeline.service';
import { CreatedTweet } from '../../Models/Tweet.model';
import { CommonModule } from '@angular/common';
import { TweetComponent } from '../tweet/tweet.component';
import { BookmarkService } from '../../Services/Bookmark.service';
import { FollowService } from '../../Services/Follow.service';
import { CreatedUser, UserForUpdate } from '../../Models/User.model';
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
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements AfterViewInit {
  requestParameters: RequestParameters = {
    PageNumber: 1,
    PageSize: 6,
    OrderBy: '',
    Fields: ''
  };
  userId: number = 0;
  userForUpdate: UserForUpdate = {
    Id: 0,
    UserName: '',
    Bio: '',
    CoverPicture: '',
    Profilepicture: ''
  };
  profileTweets = new Set<CreatedTweet>();
  followers: CreatedUser[] = [];
  followees: CreatedUser[] = [];
  user!: CreatedUser;
  count: number = 0;
  showTweetPost: boolean = false;
  tweetForUpdate!: CreatedTweet;
  allTweets = new Set<number>();
  bookmarkes = new Set<number>();
  likedTweets = new Set<number>();
  userRetweets = new Set<number>();

  @ViewChild('posts') posts!: ElementRef<HTMLButtonElement>;
  @ViewChild('likes') likes!: ElementRef<HTMLButtonElement>;
  @ViewChild('bookmarks') bookmarks!: ElementRef<HTMLButtonElement>;
  @ViewChild('retweets') retweets!: ElementRef<HTMLButtonElement>;

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
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((paramMap) => {
      this.userId = +paramMap.get('id')!;
      this.fetchProfileData();
    });
  }

  ngAfterViewInit(): void {
    this.activeButton = this.posts;
    this.getUserBookmarks();
    this.getUserLikedTweets();
    this.getUserRetweets();
    this.getUserProfile();
  }

  toggleLoading = () => (this.isLoading = !this.isLoading);

  onScroll = () => {
    if (this.activeButton == this.posts) this.getUserProfile();
  };

  fetchProfileData(): void {
    this.getUser();
    this.getFollowers();
    this.getFollowees();
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
            this.profileTweets.add(tweet);
            this.allTweets.add(tweet.id);
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
          data.forEach((tweet) => this.bookmarkes.add(tweet));
          this.count = this.bookmarkes.size;
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
          data.forEach((tweet) => this.likedTweets.add(tweet));
          this.count = this.likedTweets.size;
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
          data.forEach((tweet) => this.userRetweets.add(tweet));
          this.count = this.userRetweets.size;
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
    this.profileTweets.delete(tweet);
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


}
