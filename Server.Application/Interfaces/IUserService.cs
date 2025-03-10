using System.Linq.Expressions;
using Server.Application.Repositories;
using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTO.User;
using Server.Domain.Entities;

namespace Server.Application.Interfaces
{
    public interface IUserService 
    {
        Task<IList<User>> GetALl();
        Task<User> GetByEmail(string email);
        Task UpdateUserAsync(User user);


        Task<UserDTO> GetUserById(Guid id);
        Task<Result<User>> GetCurrentUserById();
    }
}
