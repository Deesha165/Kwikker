import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NotificationService } from '../../Services/Notification.service';
import { AuthenticationService } from '../../Services/Authentication.service';

@Component({
  selector: 'app-side-bar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './side-bar.component.html',
  styleUrl: './side-bar.component.css'
})
export class SideBarComponent {

  

userId:number=0;

notificationCount!: number;
constructor(private router:Router,private notificationService: NotificationService,private authService:AuthenticationService){
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
}

 // Reset notification count when viewing notifications
 viewNotifications() {
  this.notificationService.resetNotificationCount();

  this.router.navigate(['notifications',this.userId]);

}


goToProfile(userId:number):void{

  this.router.navigate(['profile',userId]);
  }

  logOut(){
    this.authService.logout();
    this.router.navigate(['login']);
  }
}
