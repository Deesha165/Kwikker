import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Token, UserForLogIn, UserForRegistration } from "../Models/User.model";
import { BehaviorSubject, catchError, Observable } from "rxjs";


@Injectable({
    providedIn: 'root'
})
export class AuthenticationService{
 
    private AuthenticationUrl = 'https://localhost:7246/api/Authentication';
    private TokensUrl = 'https://localhost:7246/api/Tokens';
  
    private currentUserSubject: BehaviorSubject<string | null>;
    public currentUser: Observable<string | null>;
  
    constructor(private http: HttpClient) {
      this.currentUserSubject = new BehaviorSubject<string | null>(
        localStorage.getItem('accessToken')
      );
      this.currentUser = this.currentUserSubject.asObservable();
    }

    signUp(userForRegistration:UserForRegistration ):Observable<any>{
    return this.http.post(`${this.AuthenticationUrl}`,userForRegistration);
    }
    
    login(userForLogIn: UserForLogIn): Observable<Token> {
        return this.http
          .post<Token>(`${this.AuthenticationUrl}/login`,userForLogIn )
          .pipe(
            catchError((error) => {
              console.error('Login failed', error);
              throw error;
            })
          );
      }
    


     // Save tokens to localStorage and BehaviorSubject
  setSession(authResult:Token): void {


    localStorage.setItem('accessToken', authResult.accessToken);
    localStorage.setItem('refreshToken', authResult.refreshToken);
    this.currentUserSubject.next(authResult.accessToken);
  }

  // Logout function to clear session
  logout(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    this.currentUserSubject.next(null);
  }

  // Check if the user is authenticated
  get isAuthenticated(): boolean {
    return !!localStorage.getItem('accessToken');
  }

}