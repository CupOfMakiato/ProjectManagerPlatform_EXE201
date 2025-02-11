using Microsoft.EntityFrameworkCore;
using Server.Application.Interfaces;
using Server.Application.Repositories;
using Server.Domain.Entities;
using Server.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _dbContext;

        public CategoryRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<Category>> GetCategories()
        {
            return await _dbContext.Category.Include(x => x.SubCategories).ToListAsync();
        }

        public void DeleteCategory(Category category)
        {
            _dbContext.Remove(category);
        }

        public async Task<Category> GetCategoryById(Guid categoryId)
        {
            return await _dbContext.Category.Include(x => x.SubCategories).Where(p => p.Id == categoryId).FirstOrDefaultAsync();
        }

        public async Task<Category> GetCategoryByName(string name)
        {
            return await _dbContext.Category.Include(x => x.SubCategories).Where(p => p.CategoryName == name).FirstOrDefaultAsync();
        }
    }
}
