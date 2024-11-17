using Service.Contracts.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IServiceManager
    {
        IUserService UserService { get; }
        ITweetService TweetService { get; }
        IRetweetService RetweetService { get; }
        ILikeService LikeService { get; }
        IFollowService FollowService { get; }
       IBookmarkService BookmarkService { get; }
        ITrendService trendService { get; }
       ITimelineService TimelineService { get; }    
        INotificationService NotificationService { get; }

        IAuthenticationService AuthenticationService { get; }


    }
}
