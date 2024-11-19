import { Component, Input } from '@angular/core';
import { AuthenticationService } from '../../Services/Authentication.service';
import { UserForRegistration } from '../../Models/User.model';
import { FormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpResponse } from '@microsoft/signalr';
import { HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-sign-up',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './sign-up.component.html',
  styleUrl: './sign-up.component.css'
})
export class SignUpComponent {



userForRegistration:UserForRegistration={Email:'',Password:'',UserName:''};
errors: { [key: string]: string } = {};
  constructor(private authService:AuthenticationService, private route:Router){}

  onSubmit() {
    this.errors={};
    this.authService.signUp(this.userForRegistration).subscribe({
      next:(response)=>{
               console.log(response);
               this.route.navigate(['login']);
      },
      error:(error:HttpErrorResponse)=>{
        if (error.status === 400 &&error.error) {
          this.errors = error.error; // Assigns the ModelState error object to `errors`
        } else {
          console.error('An unexpected error occurred:', error);
        }
      }
    });
  }

  goToLogIn()
  {
    this.route.navigate(['login']);
  }
}
