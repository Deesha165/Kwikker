import { HttpClient, HttpHeaders, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, Observable } from "rxjs";
import { CreatedTweet } from "../Models/Tweet.model";
import { RequestParameters } from "../Models/RequestParameters.model";

@Injectable({
    providedIn: 'root' // Add this to make the service a singleton across the app
  })
export class TimelineService
{
    private TimelinesUrl='https://localhost:7246/api/Timelines';
    public headers: HttpHeaders;
    constructor(private http:HttpClient)
    {
        this.headers = new HttpHeaders({
            Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
      'Content-Type': 'application/json'
          });      
    }


    getFollowersNews(userId:number):Observable<CreatedTweet[]>
    {

         return this.http.get<CreatedTweet[]>
         (`${this.TimelinesUrl}/followed/${userId}`,{headers:this.headers});
    }
    
    getRandomTimeline(userId:number):Observable<CreatedTweet[]>{
    return this.http.get<CreatedTweet[]>
    
    (`${this.TimelinesUrl}/random/${userId}`,{headers:this.headers});
  }

  getUserLikedTweets(userId: number): Observable< number[]> {

    console.log('hello from timeline service');
    return this.http.get<number[]>(`${this.TimelinesUrl}/LikedTweets/${userId}`,{headers:this.headers});
}

getUserRetweets(userId: number): Observable< number[]> {

    console.log('hello from timeline service');
    return this.http.get<number[]>(`${this.TimelinesUrl}/retweets/${userId}`,{headers:this.headers});
}
getProfile(userId: number, requestParameters: RequestParameters): Observable<{ tweets: CreatedTweet[], totalCount: number }> {
  return this.http.get<CreatedTweet[]>
  (`${this.TimelinesUrl}/profile/${userId}?PageNumber=${requestParameters.PageNumber}&PageSize=${requestParameters.PageSize} `, {
    headers: this.headers,
      observe: 'response'
  }).pipe(
      map((response: HttpResponse<CreatedTweet[]>) => {
          // Extract metadata from X-Pagination header
          const paginationHeader = response.headers.get('X-Pagination');
          let totalCount = 0;

          if (paginationHeader) {
              const metaData = JSON.parse(paginationHeader);
              totalCount = metaData.TotalCount || 0;
              console.log(metaData);
          }

          return {
              tweets: response.body || [],
              totalCount
          };
      })
  );
}


}