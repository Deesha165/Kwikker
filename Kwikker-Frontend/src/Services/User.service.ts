import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { CreatedTweet, TweetForCreation } from "../Models/Tweet.model";
import { Observable } from "rxjs";
import { CreatedUser } from "../Models/User.model";

@Injectable({
  providedIn: 'root' // Add this to make the service a singleton across the app
})
export class UserService{

    private Url='https://localhost:7246/api/Users';
    public headers: HttpHeaders;
    constructor(private http:HttpClient)
    {
      this.headers = new HttpHeaders({
        Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
  'Content-Type': 'application/json'
      });            
    }

    getUser(userId: number): Observable<CreatedUser> {
        return this.http.get<CreatedUser>(`${this.Url}/${userId}`,{headers:this.headers});
      }

   

}