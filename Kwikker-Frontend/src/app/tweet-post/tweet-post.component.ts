import { Component, ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { CreatedTweet, TweetForCreation, TweetForUpdate } from '../../Models/Tweet.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-tweet-post',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './tweet-post.component.html',
  styleUrl: './tweet-post.component.css'
})
export class TweetPostComponent {


constructor (){}

tweets!:CreatedTweet[];


@Input() currentTweet!:CreatedTweet;

@Output() newTweet=new EventEmitter<CreatedTweet>();

 
  @ViewChild('fileInput') fileInput!: ElementRef;

  isTweetValid: boolean = false;
  checkTweetContent(): void {
    this.isTweetValid = this.currentTweet.content.trim().length > 0;
  }
  sendTweet(): void {
    
     this.newTweet.emit(this.currentTweet);
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
          
          this.currentTweet.mediaUrl = e.target.result;
        };
  
        reader.readAsDataURL(file);  // Read the file as Data URL (for image preview)
  
      }
    }
    @Output() close = new EventEmitter<void>();
    closePopup() {
      this.close.emit(); // Emit close event when close button is clicked
    }
}
