import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UserForLogIn } from '../../Models/User.model';
import { AuthenticationService } from '../../Services/Authentication.service';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { Token } from '@angular/compiler';
@Component({
  selector: 'app-log-in',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './log-in.component.html',
  styleUrl: './log-in.component.css'
})
export class LogInComponent {

  userForLogIn:UserForLogIn={
    Email:'',
    Password:''
  }
  errors: { [key: string]: string } = {};
  constructor(private authService:AuthenticationService, private route:Router){}

onSubmit() {
this.authService.login(this.userForLogIn).subscribe({
  next:(response)=>{
  this.errors={};
   console.log(response);
              this.authService.setSession(response);
             
               this.route.navigate(['home']);
  },
  error:(error:HttpErrorResponse)=>{
    if (error.status === 401 ) {
    
      this.errors['data']="Invalid Email or Password";
      console.log(this.errors['data']);
    } else {
      console.error('An unexpected error occurred:', error);
    }
  }
});
}
goToSignUp()
{
this.route.navigate(['signup'])
}

}
