import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationService } from '../../Services/Notification.service';
import { CommonModule } from '@angular/common';
import { CreatedNotification } from '../../Models/Notification.model';
import { AuthenticationService } from '../../Services/Authentication.service';

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './notifications.component.html',
  styleUrl: './notifications.component.css'
})
export class NotificationsComponent {
notificationEntities!:CreatedNotification[];
  userId=0;
constructor( private route:ActivatedRoute,private router:Router, 
  private notificationService :NotificationService,private authService: AuthenticationService)
  {
    
  }

ngOnInit():void{
  this.route.paramMap.subscribe(paramMap => {
    this.userId = +paramMap.get('id')!; // Extract userId from route
      this.getNotifications();
  });
}

getNotifications():void{
console.log('hello from notifications');
this.authService.handleUnauthorized(()=>this.notificationService.getNotifications(this.userId))
.subscribe((data)=>{
  this.notificationEntities=data;

})
}

goToProfile(senderId:number)
{
  this.router.navigate(['profile',senderId])
}


}
