import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AuthenticationResponse, Token, UserForLogIn, UserForRegistration } from "../Models/User.model";
import { BehaviorSubject, catchError, Observable, switchMap, tap, throwError } from "rxjs";
import { Router } from "@angular/router";


@Injectable({
    providedIn: 'root'
})
export class AuthenticationService{
 
    private AuthenticationUrl = 'https://localhost:7246/api/Authentication';
    private TokensUrl = 'https://localhost:7246/api/Tokens';
  
  
    constructor(private http: HttpClient,private route:Router) {
     
    }

    signUp(userForRegistration:UserForRegistration ):Observable<any>{
    return this.http.post(`${this.AuthenticationUrl}`,userForRegistration);
    }
    
    login(userForLogIn: UserForLogIn): Observable<AuthenticationResponse> {
        return this.http
          .post<AuthenticationResponse>(`${this.AuthenticationUrl}/login`,userForLogIn )
          .pipe(
            catchError((error) => {
              console.error('Login failed', error);
              throw error;
            })
          );
      }
    
//get Token from local storage
getToken():Token{

return {
  accessToken:localStorage.getItem('accessToken'),
    refreshToken:localStorage.getItem('refreshToken')
}

}

//get new access token to preserve session
setNewAccessToken(oldToken:Token):Observable<Token>{

   return this.http.post<Token>(`${this.TokensUrl}/refresh`, oldToken);
}

     // Save tokens to localStorage and BehaviorSubject
  setToken(authResult:Token): void {
    
    if(authResult.accessToken !=null&& authResult.refreshToken!=null)
    {
      localStorage.setItem('accessToken', authResult.accessToken);
      localStorage.setItem('refreshToken', authResult.refreshToken);
     
    }

    
  }

  // Logout function to clear session
  logout(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('userId');
    this.route.navigate(['login']);
   
  }

  // Check if the user is authenticated
   isAuthenticated(): boolean {
    const token = localStorage.getItem('accessToken');
    if (!token) return false;
    return true;
  }


  handleUnauthorized<T>(operation: () => Observable<T>): Observable<T> {
    return operation().pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          console.log('Protected resource unauthorized');
  
          const token = this.getToken();
          if (token.accessToken) {
            return this.setNewAccessToken(token).pipe(
              tap((newToken) => {
                // Set the new token if the response is valid
                console.log('New access token received:', newToken);
                this.setToken(newToken);
              }),
              switchMap(() => operation()), // Retry the original operation
              catchError((refreshError: HttpErrorResponse) => {
                // Check the error message for refresh token failure
                if ( refreshError.status===401&& refreshError.error?.Message === 'Refresh token is expired. Please log in again.') {
                  console.log('Refresh token is expired. Logging out...');
                  this.logout();
                } else {
                  console.error('Error during token refresh:', refreshError);
                }
                return throwError(() => refreshError); // Propagate the error
              })
            );
          }
        }
  
        return throwError(() => error); // Propagate other errors
      })
    );
  }
  

  
}