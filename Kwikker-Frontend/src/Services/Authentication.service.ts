import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AuthenticationResponse, Token, UserForLogIn, UserForRegistration } from "../Models/User.model";
import { BehaviorSubject, catchError, Observable, switchMap, tap, throwError } from "rxjs";


@Injectable({
    providedIn: 'root'
})
export class AuthenticationService{
 
    private AuthenticationUrl = 'https://localhost:7246/api/Authentication';
    private TokensUrl = 'https://localhost:7246/api/Tokens';
  
  
    constructor(private http: HttpClient) {
     
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
   
  }

  // Check if the user is authenticated
  get isAuthenticated(): boolean {
    return !!localStorage.getItem('accessToken');
  }


  handleUnauthorized<T>(operation: () => Observable<T>): Observable<T> {
    return operation().pipe(
      catchError((error) => {
        if (error.status === 401) {

          console.log('protecte resource unauthorized');
          const token = this.getToken();
          if (token.accessToken) {
            return this.setNewAccessToken(token).pipe(
              tap((newToken) => this.setToken(newToken)),
              switchMap(() => operation()) // Retry original operation
            );
          }
        }
        return throwError(() => error); // Propagate other errors
      })
    );
  }

  
}