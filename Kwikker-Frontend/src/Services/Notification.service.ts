import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';
import { CreatedNotification } from '../Models/Notification.model';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {

  private NotificationsUrl='https://localhost:7246/api/Notifications';

  private hubConnection!: signalR.HubConnection;
  private notificationCount = new BehaviorSubject<number>(0);
  notificationCount$ = this.notificationCount.asObservable();

  constructor(private http:HttpClient) {
    this.startConnection();
    this.addNotificationListener();
  }

  // Start SignalR connection
  private startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7246/notificationHub', {
        withCredentials: true // Ensures the browser includes credentials with the request
      }) // Replace with your API URL
      .build();

    this.hubConnection
      .start()
      .catch(err => console.error('Error while starting SignalR connection: ' + err));
  }

  // Listen for notifications
  private addNotificationListener() {
    this.hubConnection.on('ReceiveNotification', (message: string) => {
      // Update the notification count
      this.notificationCount.next(this.notificationCount.value + 1);
      console.log('New Notification:', message);
    });
  }

  // Reset notification count
  resetNotificationCount() {
    this.notificationCount.next(0);
  }



  getNotifications(receiverId: number): Observable< CreatedNotification[]> {
    return this.http.get<CreatedNotification[]>(`${this.NotificationsUrl}/user/${receiverId}`);
}

}
