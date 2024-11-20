import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NotificationService } from '../../Services/Notification.service';
import { AuthenticationService } from '../../Services/Authentication.service';
import { CreatedUser } from '../../Models/User.model';
import { UserService } from '../../Services/User.service';

@Component({
  selector: 'app-side-bar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './side-bar.component.html',
  styleUrl: './side-bar.component.css'
})
export class SideBarComponent {

  

userId:number=0;

user!:CreatedUser;

notificationCount!: number;
constructor(private router:Router,private notificationService: NotificationService
  ,private authService:AuthenticationService, private userService:UserService)
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
}
