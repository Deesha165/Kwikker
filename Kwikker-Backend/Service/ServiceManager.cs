using Contracts;
using Service.Contracts.Contracts;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.ServiceModels;
using AutoMapper;
using Shared.DTOs;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using Microsoft.AspNetCore.SignalR;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Service
{
    public sealed class ServiceManager : IServiceManager
    {
       
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<ITweetService> _tweetService;
        private readonly Lazy<IRetweetService> _retweetService;
        private readonly Lazy<ILikeService> _likeService;
        private readonly Lazy<IFollowService> _followService;
        private readonly Lazy<IBookmarkService> _bookmarkService;

        private readonly Lazy<ITrendService> _trendService;
        private readonly Lazy<ITimelineService> _timelineService;

        private readonly Lazy<INotificationService> _notificationService;
        private readonly Lazy<IAuthenticationService> _authenticationService;
        public ServiceManager
          (IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper,
            IConnectionMultiplexer redisConnection, IDataShaper<TweetDTO> dataShaper,
            IHubContext<NotificationHub> hubContext, UserManager<User> userManager,IConfiguration configuration)
        {
            
            _userService = new Lazy<IUserService>(() => new
            UserService(repositoryManager, logger,mapper));

            _tweetService = new Lazy<ITweetService>(() => new
            TweetService(repositoryManager, logger,mapper, dataShaper));

            _notificationService = new Lazy<INotificationService>(() => new
                NotificationService(repositoryManager, logger, mapper));

            _retweetService = new Lazy<IRetweetService>(() => new
          RetweetService(repositoryManager, logger,mapper,redisConnection, hubContext, _notificationService.Value));

            _likeService = new Lazy<ILikeService>(() => new
            LikeService(repositoryManager, logger,mapper,redisConnection, hubContext, _notificationService.Value));

            _followService = new Lazy<IFollowService>(() => new
          FollowService(repositoryManager, logger,mapper,redisConnection,hubContext,_notificationService.Value,_userService.Value));

            _bookmarkService = new Lazy<IBookmarkService>(() => new
          BookmarkService(repositoryManager, logger, mapper, redisConnection));

            _trendService = new Lazy<ITrendService>(() => new
          TrendService(repositoryManager, logger, mapper,redisConnection));

            _timelineService = new Lazy<ITimelineService>(() => new
           TimelineService(repositoryManager, logger, mapper,_followService.Value,_tweetService.Value));

            _authenticationService = new Lazy<IAuthenticationService>(() =>
                             new AuthenticationService(logger, mapper, userManager,configuration));


        }


        public IUserService UserService => _userService.Value;

        public ITweetService TweetService =>_tweetService.Value;

        public IRetweetService RetweetService => _retweetService.Value;

        public ILikeService LikeService => _likeService.Value;

        public IFollowService FollowService => _followService.Value;

        public IBookmarkService BookmarkService => _bookmarkService.Value;

        public ITrendService trendService => _trendService.Value;

        public ITimelineService TimelineService => _timelineService.Value;

        public INotificationService NotificationService => _notificationService.Value;

        public IAuthenticationService AuthenticationService => _authenticationService.Value;
    }
}
