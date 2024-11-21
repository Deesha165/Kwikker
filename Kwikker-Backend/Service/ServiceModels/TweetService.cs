using AutoMapper;
using Contracts;
using Entities.DomainModels;
using Entities.ExceptionModels;
using Entities.Models;
using Service.Contracts.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Service.ServiceModels
{
    internal sealed class TweetService:ITweetService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        
        public TweetService(IRepositoryManager repository, ILoggerManager
        logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper=mapper;
       
        }

        

        public async Task<TweetDTO> CreateTweet(int UserId, TweetForCreationDTO tweet, bool trackChanges)
        {
            var user =await _repository.UserRepository.GetUser(UserId,trackChanges);
            
            if (user is null) throw new NotFoundException(UserId,"User");
            var tweetEntity = _mapper.Map<Tweet>(tweet);
            

           

            _repository.TweetRepository.CreateTweet(UserId, tweetEntity);
            await _repository.SaveAsync();

            
            await AddNewTrends(tweet.content,tweetEntity.ID);


            var tweetToReturn = 
                new TweetDTO(tweetEntity.ID,tweetEntity.Content!,UserId,tweetEntity.CreatedAt,
                user.UserName!,0,0,0,user.Email!,user.ProfilePicture!,tweetEntity.MediaURL!);



            return tweetToReturn;
        }

        public async Task DeleteTweet(int id,bool trackChanges)
        {
            var tweetEntity =await _repository.TweetRepository.GetTweet(id,trackChanges);
            
            if (tweetEntity is null) throw new NotFoundException(id, "Tweet");

            
            _repository.TweetRepository.DeleteTweet(tweetEntity);
            await _repository.SaveAsync();
          
        }

        public async Task<TweetDTO> GetTweet(int id, bool trackChanges)
        {
            
                var tweet = await   _repository.TweetRepository.GetTweet(id, trackChanges);

                if (tweet is null)
                    throw new NotFoundException(id, "Tweet");

                var TweetDTO = _mapper.Map<TweetDTO>(tweet);
                return TweetDTO;
           
        }
        public async Task<TweetDTO> UpdateTweet(TweetForUpdateDTO tweetForUpdateDTO)
        {
            // 1. Retrieve the existing tweet and track changes
            var tweet = await _repository.TweetRepository.GetTweet(tweetForUpdateDTO.id, trackChanges: true);
            if (tweet is null)
                throw new NotFoundException($"Tweet with id {tweetForUpdateDTO.id} doesn't exist.");

            // 2. Map the changes from the DTO onto the existing tracked tweet entity
            _mapper.Map(tweetForUpdateDTO, tweet);
            tweet.UpdatedAt = DateTime.Now;

            // 3. Save changes
            await _repository.SaveAsync();

            // 4. Map the updated tweet back to DTO and return
            var updatedTweetDTO = _mapper.Map<TweetDTO>(tweet);
            return updatedTweetDTO;
        }


        public async Task<(IEnumerable<TweetDTO> tweets, MetaData metaData)>
            GetTweetsByUser(int UserId,TweetParameters tweetParameters,bool trackChanges)
        {
            var tweetsWithMetaData =await _repository.TweetRepository.GetTweetsByUser(UserId,  tweetParameters,trackChanges);

            if (!tweetsWithMetaData.Any())
            {
                return (Enumerable.Empty<TweetDTO>(), new MetaData());
            }

            var TweetDTOs = _mapper.Map<IEnumerable<TweetDTO>>(tweetsWithMetaData);

          

            return (tweets: TweetDTOs, metaData: tweetsWithMetaData.MetaData);

        }
        private async Task AddNewTrends(string tweet,int tweetId)
        {
            var hashtags = ExtractHashtags(tweet);
           
            var existingTrends=await _repository.TrendRepository.GetExistingTrends(hashtags);

            foreach(var existingTrend in existingTrends)
            {
                var trend = existingTrend.Value;
                trend.Occurrences++;
                trend.LastOccurred = DateTime.Now;
                trend.DecayScore = CalculateDecayScore(trend.Occurrences, trend.LastOccurred);
            }
            List<Trend> newTrends=new List<Trend>();
            foreach(var trend in hashtags)
            {
                var exist = existingTrends.ContainsKey(trend);
                if(!exist)
                {
                    newTrends.Add(new Trend()
                    {
                        hashtag = trend,
                        Occurrences = 1,
                        LastOccurred = DateTime.Now,
                        DecayScore = CalculateDecayScore(1, DateTime.Now)
                    }
                    );
                }
              
            }
           
          await _repository.TrendRepository.CreateNewTrendsAsync(newTrends);


            List<TweetTrend> tweetTrends = new List<TweetTrend>();  
            foreach(var newtrend in newTrends)
            {
                tweetTrends.Add(new TweetTrend()
                {
                    TrendId = newtrend.Id,
                    TweetId = tweetId
                });
                
            }
            foreach (var existingTrend in existingTrends)
            {
                tweetTrends.Add(
                    new TweetTrend()
                    {
                        TrendId=existingTrend.Value.Id,
                        TweetId=tweetId
                    });
            }
            await _repository.TweetTrendRepository.CreateNewTweetTrendsAsync(tweetTrends);

            await _repository.SaveAsync();
        }
        private  List<string> ExtractHashtags(string tweetContent)
        {
            // Basic hashtag extraction logic using Regex
            var hashtagRegex = new Regex(@"#\w+");
            return hashtagRegex.Matches(tweetContent).Select(m => m.Value.ToLower()).ToHashSet().ToList();
        }
        private  double CalculateDecayScore(int occurrences, DateTime lastOccurred)
        {
            TimeSpan timeDifference = DateTime.Now - lastOccurred;
            double timeElapsed = timeDifference.TotalHours;
            return occurrences / (1 + timeElapsed);
        }

      
    }

    }

