using Shared.DTOs;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts.Contracts
{
    public interface IUserService
    {
        Task<ExpandoObject> GetUser(int id,bool trackChanges,UserParameters userParameters);

        Task<UserForUpdateDTO> UpdateUser(UserForUpdateDTO userForUpdateDTO);
        Task<int> GetUserCount();   
    }
}
