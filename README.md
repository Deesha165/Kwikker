# Kwikker

Kwikker is a dynamic social media application designed to facilitate seamless interaction among users through features such as tweeting, liking, and real-time notifications.

##  Tech Stack

- **Languages**: C#, TypeScript.
- **Frameworks**: ASP.NET Core, Angular, Entity Framework Core.
- **Databases**: SQL Server, Redis (for caching).
- **Real-time Communication**: SignalR.
- **Tools**: Git, Postman, Hangfire (for background tasks), AutoMapper, LINQ, Dynamic LINQ.
  
##  Features

- **Tweeting System**: Users can create, read, update, and delete tweets, retweeting and bookmarking.
- **Following users**: Added ability to follow and unfollow users and suggest news based on these followings.
- **Liking Tweets**: Users can like tweets, triggering real-time notifications to the tweet's owner.
- **Real-Time Notifications**: Instant notifications for user interactions, leveraging SignalR.
- **Trends**: Ability to update most recent and popular trends frequently using background jobs.
- **Caching Timelines**: Added caching for timelines using Redis to improve performance.
- **Infinite Scrolling**: Added infinite scrolling and paging tweets for better user experience.
- **User Authentication**: Secure authentication with JWT tokens.  

## Restful Api Design

- **Paging**: For efficient Api data fetching.
- **Data Shaping**: For dynamic Api data fetching.

##  Design Patterns

- **Lazy Instantiation**: To manage Services and Repositories to be instantiated only when required for better performance.  
- **Repository Pattern**: To abstract data access and make the application more modular.
- **Dependency Injection**: To manage dependencies and enhance testability.
  

