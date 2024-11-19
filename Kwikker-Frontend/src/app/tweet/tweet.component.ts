import { Component, EventEmitter, Input, Output } from '@angular/core';
import { TweetService } from '../../Services/Tweet.service';
import { CreatedTweet } from '../../Models/Tweet.model';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { BookmarkService } from '../../Services/Bookmark.service';
import { AuthenticationService } from '../../Services/Authentication.service';

@Component({
  selector: 'app-tweet',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  templateUrl: './tweet.component.html',
  styleUrls: ['./tweet.component.css']
})
export class TweetComponent {
  @Input() tweet!: CreatedTweet;
 
  @Input() isRetweeted = false;
  @Input() isLiked = false;
  @Input() isBookmarked = false;

  @Output() tweetDeleted = new EventEmitter<CreatedTweet>();
  @Output() updateTweet = new EventEmitter<CreatedTweet>();

  userId = 0;
  isDropdownOpen = false;

  constructor(
    private tweetService: TweetService,
    private bookmarkService: BookmarkService,
    private authService: AuthenticationService
  ) { 
    
    const storedUserId = localStorage.getItem('userId');

    // Check if 'userId' exists and is a valid number
    if (storedUserId) {
      this.userId = parseInt(storedUserId, 10); // parseInt with base 10
    }
  }

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  updateLike(): void {
    const operation = this.isLiked
      ? () => this.tweetService.removeLike(this.userId, this.tweet.id)
      : () => this.tweetService.createLike(this.userId, this.tweet.id);

    this.authService.handleUnauthorized(operation).subscribe({
      next: () => {
        this.tweet.likesNumber += this.isLiked ? -1 : 1;
        this.isLiked = !this.isLiked;
      },
      error: (error) => console.error('Failed to update like:', error)
    });
  }

  updateBookmark(): void {
    const operation = this.isBookmarked
      ? () => this.bookmarkService.removeBookmark(this.userId, this.tweet.id)
      : () => this.bookmarkService.createBookmark(this.userId, this.tweet.id);

    this.authService.handleUnauthorized(operation).subscribe({
      next: () => {
        this.tweet.bookmarksNumber += this.isBookmarked ? -1 : 1;
        this.isBookmarked = !this.isBookmarked;
      },
      error: (error) => console.error('Failed to update bookmark:', error)
    });
  }

  updateRetweet(): void {
    const operation = this.isRetweeted
      ? () => this.tweetService.removeRetweet(this.userId, this.tweet.id)
      : () => this.tweetService.createRetweet(this.userId, this.tweet.id);

    this.authService.handleUnauthorized(operation).subscribe({
      next: () => {
        this.tweet.retweetsNumber += this.isRetweeted ? -1 : 1;
        this.isRetweeted = !this.isRetweeted;
      },
      error: (error) => console.error('Failed to update retweet:', error)
    });
  }

  editTweet(): void {
    this.updateTweet.emit(this.tweet);
    console.log('Edit tweet clicked');
  }

  deleteTweet(): void {
    this.authService
      .handleUnauthorized(() => this.tweetService.removeTweet(this.tweet.id))
      .subscribe({
        next: () => this.tweetDeleted.emit(this.tweet),
        error: (error) => console.error('Failed to delete tweet:', error)
      });
  }
}
