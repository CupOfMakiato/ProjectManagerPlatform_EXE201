using System.Linq.Expressions;
using Server.Domain.Entities;

namespace Server.Application.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<IList<User>> GetALl();
        Task<User?> FindByEmail(string email);
        Task UpdateAsync(User user);
        Task AddAsync(User user);
        Task<User> GetUserByEmail(string email);
        Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate);
        Task<User> GetUserById(Guid userId);
        Task<User> GetAllUserById(Guid id);
        Task<List<User>> GetUsersByRole(int role);
        //Task<User> GetUserByIdWithServiceUsed(Guid userId);

        Task<User> GetUserByVerificationToken(string token);

        //Forget Password
        Task<User> GetUserByResetToken(string resetToken);
    }
}
