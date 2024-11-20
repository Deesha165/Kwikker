using AutoMapper;
using Contracts;
using Entities.ExceptionModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Service.Contracts.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ServiceModels
{
    internal sealed class UserService:IUserService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<User> _shapper;
        private readonly UserManager<User> _userManager;
        public UserService(IRepositoryManager repository, ILoggerManager
        logger,IMapper mapper, IDataShaper<User> shapper, UserManager<User> userManager)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _shapper = shapper;
            _userManager = userManager;
        }

        public async Task< ExpandoObject> GetUser(int id, bool trackChanges,UserParameters userParameters)
        {
          
                var user =await _repository.UserRepository.GetUser(id, trackChanges);
                
                if (user is null)
                    throw new NotFoundException(id,"User");

            var shapedUser = _shapper.ShapeData(user, userParameters.Fields!);
                
            return shapedUser;
            
        }

        public async Task<int> GetUserCount()
        =>await _repository.UserRepository.GetUsersCount();

        public async Task<UserForUpdateDTO> UpdateUser(UserForUpdateDTO userForUpdateDTO)
        {
            var user = await _userManager.FindByIdAsync(userForUpdateDTO.Id.ToString());
            if (user == null)
                throw new NotFoundException($"User with id {userForUpdateDTO.Id} doesn't exist.");

            // Update properties
            user.Bio = userForUpdateDTO.Bio;
            user.UserName = userForUpdateDTO.UserName;
            user.ProfilePicture = userForUpdateDTO.ProfilePicture;
            user.CoverPicture = userForUpdateDTO.CoverPicture;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return userForUpdateDTO;
        }

    }
}
