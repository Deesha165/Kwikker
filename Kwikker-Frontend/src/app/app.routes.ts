import { Routes } from '@angular/router';
import { HomeTimelineComponent } from './home-timeline/home-timeline.component';
import { ProfileComponent } from './profile/profile.component';
import { FollowersComponent } from './followers/followers.component';
import { FolloweesComponent } from './followees/followees.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { TrendingListComponent } from './trending-list/trending-list.component';
import { SignUpComponent } from './sign-up/sign-up.component';
import { LogInComponent } from './log-in/log-in.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = 
[   {path:'home',component:HomeTimelineComponent,canActivate:[authGuard]} , 
    {path:'profile/:id',component:ProfileComponent,canActivate:[authGuard]},
    {path:'followers/:id',component:FollowersComponent,canActivate:[authGuard]},
    {path:'followees/:id',component:FolloweesComponent,canActivate:[authGuard]},
    {path:'notifications/:id',component:NotificationsComponent,canActivate:[authGuard]},
    {path:'trends/:hashtag/:id' , component:TrendingListComponent,canActivate:[authGuard]},
    {path:'signup',component:SignUpComponent},
    {path:'login',component:LogInComponent},
    { path: '**', redirectTo: '/signup' }

];
