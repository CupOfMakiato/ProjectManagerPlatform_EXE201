using Server.Contracts.DTO.User;
using Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Mappers.UserExtension
{
    public static class UserExtension
    {
        public static UserDTO ToUserDTO(this User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Balance = null,
            };
        }
        //public static SearchServiceUserDTO ToSearchUserDTO(this User user)
        //{
        //    return new SearchServiceUserDTO
        //    {
        //        Id = user.Id,
        //        UserName = user.UserName,
        //        PhoneNumber = user.PhoneNumber,
        //    };
        //}
    }
}
