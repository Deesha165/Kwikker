import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class BookmarkService {

    private BookmarksUrl = 'https://localhost:7246/api/Bookmarks';
    public headers: HttpHeaders;
    constructor(private http: HttpClient) {  this.headers = new HttpHeaders({
      Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
'Content-Type': 'application/json'
    });
  
  }

    getBookmarks(userId: number): Observable< number[]> {
      return this.http.get<number[]>(`${this.BookmarksUrl}/${userId}`,{headers:this.headers});

    }
    createBookmark(userId:number,tweetId:number):Observable<any>{
        return this.http.post(`${this.BookmarksUrl}/${userId}/${tweetId}`,[],{headers:this.headers});
      }
      removeBookmark(userId:number,tweetId:number):Observable<any>{
        return this.http.delete(`${this.BookmarksUrl}/${userId}/${tweetId}`,{headers:this.headers});
      }
  
    
}
