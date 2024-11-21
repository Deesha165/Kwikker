import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { CreatedUser, CustomUser } from "../Models/User.model";
import { Observable } from "rxjs";
import { CreatedTweet } from "../Models/Tweet.model";

@Injectable({
    providedIn:'root'
})
export class FollowService{

    private FollowingsUrl='https://localhost:7246/api/Followings';
    public headers: HttpHeaders;
    constructor(private http:HttpClient){
        this.headers = new HttpHeaders({
            Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
      'Content-Type': 'application/json'
          });
    }
 getSuggestedUsersToFollowDynamic(userId:number): Observable<CreatedUser[]>{
        return this.http.get<CreatedUser[]>(`${this.FollowingsUrl}/random/${userId}?fields=Id,ProfilePicture,Email,UserName`,{headers:this.headers});
    }

    getFollowers(userId:number):Observable<CustomUser[]>{
        return this.http.get<CustomUser[]>(`${this.FollowingsUrl}/${userId}/followers`,{headers:this.headers});
    }
    getFollowees(userId:number):Observable<CustomUser[]>{
        return this.http.get<CustomUser[]>(`${this.FollowingsUrl}/${userId}/followees`,{headers:this.headers});
    }

    createFollow(followerId:number,followeeId:number):Observable<any>{
         
        return this.http.post(`${this.FollowingsUrl}/${followerId}/follows/${followeeId}`,[],{headers:this.headers});
      }

   removeFollow(followerId:number,followeeId:number):Observable<any>{
    return this.http.delete(`${this.FollowingsUrl}/${followerId}/unfollows/${followeeId}`,{headers:this.headers});
   }   
}