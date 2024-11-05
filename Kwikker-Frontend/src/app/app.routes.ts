import { Routes } from '@angular/router';
import { HomeTimelineComponent } from './home-timeline/home-timeline.component';
import { ProfileComponent } from './profile/profile.component';
import { FollowersComponent } from './followers/followers.component';
import { FolloweesComponent } from './followees/followees.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { Component } from '@angular/core';
import { TrendingListComponent } from './trending-list/trending-list.component';

export const routes: Routes = 
[   {path:'home',component:HomeTimelineComponent} , 
    {path:'profile/:id',component:ProfileComponent},
    {path:'followers/:id',component:FollowersComponent},
    {path:'followees/:id',component:FolloweesComponent},
    {path:'notifications/:id',component:NotificationsComponent},
    {path:'trends/:hashtag/:id' , component:TrendingListComponent},
    { path: '**', redirectTo: '/home' }

];
