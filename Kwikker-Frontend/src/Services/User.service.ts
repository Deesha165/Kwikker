import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { CreatedTweet, TweetForCreation } from "../Models/Tweet.model";
import { Observable } from "rxjs";
import { CreatedUser, UserForUpdate } from "../Models/User.model";

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

    getUserDynamic1(userId: number): Observable<any> {
        return this.http.get<any>(`${this.Url}/${userId}?fields=Id,Email,UserName,Bio,ProfilePicture,CoverPicture,CreatedAt`,{headers:this.headers});
      }
      getUserDynamic2(userId:number):Observable<any>{
        return this.http.get<any>(`${this.Url}/${userId}?fields=Id,UserName,Email,ProfilePicture`,{headers:this.headers});
      }
      getUserDynamic3(userId:number):Observable<any>{
        return this.http.get<any>(`${this.Url}/${userId}?fields=Id,ProfilePicture`,{headers:this.headers});
      }


   updateUser(userForUpdate: UserForUpdate):Observable<UserForUpdate>{
               return this.http.put<UserForUpdate>(`${this.Url}`,userForUpdate,{headers:this.headers});
   }
   
}