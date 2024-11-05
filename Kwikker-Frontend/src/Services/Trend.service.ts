
import { HttpClient } from "@angular/common/http";
import { CreatedTrend} from "../Models/Trend.model";
import { Observable } from "rxjs";
import { CreatedTweet } from "../Models/Tweet.model";
import { Injectable } from "@angular/core";
@Injectable({
    providedIn: 'root',
  })
export class TrendService{

    private TrendsUrl='https://localhost:7246/api/Trends';
constructor( private http:HttpClient){}


getTrends():Observable<CreatedTrend[]>{

    return this.http.get<CreatedTrend[]>(`${this.TrendsUrl}/TopTrends`);
}
getTweetsByTrend(hashtag:string):Observable<CreatedTweet[]>{
    return this.http.get<CreatedTweet[]>(`${this.TrendsUrl}/${encodeURIComponent(hashtag)}`);
}

}