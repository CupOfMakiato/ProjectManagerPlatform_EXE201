using System.Linq.Expressions;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Server.Application.Interfaces;
using Server.Application.Repositories;
using Server.Domain.Entities;
using Server.Infrastructure.Data;

namespace Server.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }
        public async Task<IList<User>> GetALl()
        {
            return await _dbContext.User.Where(u => u.RoleId != 1).ToListAsync();
        }
        public async Task<User> FindByEmail(string email)
        {
            return await _dbContext.User
           .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _dbContext.User.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _dbContext.User.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _dbContext.User.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate)
        {
            return await _dbContext.User.AnyAsync(predicate);
        }

        public async Task<User> GetUserById(int userId)
        {
            var user = await _dbContext.User.FindAsync(userId);
            return user;
        }

        public async Task<User> GetAllUserById(Guid id)
        {
            return await _dbContext.User.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<User> GetUserById(Guid userId)
        {
            return await _dbContext.User.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> GetUserByVerificationToken(string token)
        {
            return await _dbContext.User.FirstOrDefaultAsync(u => u.VerificationToken == token);
        }
        public async Task<User> GetUserByResetToken(string resetToken)
        {
            return await _dbContext.User.FirstOrDefaultAsync(u => u.ResetToken == resetToken);
        }

        public async Task<List<User>> GetUsersByRole(int role)
        {
            return await _dbContext.User.Where(u => u.RoleId == role).ToListAsync();
        }

        public async Task<User?> FindByLoginAsync(string provider, string key)
        {
            return await _dbContext.User
            .Where(u => u.Provider == provider && u.ProviderKey == key)
            .FirstOrDefaultAsync();
        }

        public async Task<User> RegisterWithGoogleAsync(GoogleJsonWebSignature.Payload payload)
        {
            var user = new User
            {
                UserName = payload.Name,
                Email = payload.Email,
                Provider = "Google",
                ProviderKey = payload.Subject
            };

            _dbContext.User.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
    }
}
