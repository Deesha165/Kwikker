import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { CreatedTweet, TweetForCreation, TweetForUpdate } from "../Models/Tweet.model";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root' // Add this to make the service a singleton across the app
})
export class TweetService{

    private Url='https://localhost:7246/api/Tweets';
    public headers: HttpHeaders;
    constructor(private http:HttpClient)
    {
      this.headers = new HttpHeaders({
        Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
  'Content-Type': 'application/json'
      });                
    }

    getTweetsByUserId(userId: number): Observable<CreatedTweet[]> {
        return this.http.get<CreatedTweet[]>(`${this.Url}/User/${userId}`,{headers:this.headers});
      }

    createTweet(userId:number,tweetToCreate:TweetForCreation):Observable<any>{
         
      return this.http.post(`${this.Url}/${userId}`, tweetToCreate,{headers:this.headers});
    }
    updateTweet(tweetForUpdate:TweetForUpdate):Observable<CreatedTweet>{
      return this.http.put<CreatedTweet>(`${this.Url}`,tweetForUpdate,{headers:this.headers});
    }
    removeTweet(tweetId:number):Observable<any>{
      return this.http.delete(`${this.Url}/${tweetId}`,{headers:this.headers});
    }

    createLike(userId:number,tweetId:number):Observable<any>{
      return this.http.post(`${this.Url}/like/${userId}/${tweetId}`,[],{headers:this.headers});
    }
    removeLike(userId:number,tweetId:number):Observable<any>{
      return this.http.delete(`${this.Url}/like/${userId}/${tweetId}`,{headers:this.headers});
    }
    

    createRetweet(userId:number,tweetId:number):Observable<any>{
      return this.http.post(`${this.Url}/retweet/${userId}/${tweetId}`,[],{headers:this.headers});
    }

    removeRetweet(userId:number,tweetId:number):Observable<any>{
      return this.http.delete(`${this.Url}/retweet/${userId}/${tweetId}`,{headers:this.headers});
    }
    


}