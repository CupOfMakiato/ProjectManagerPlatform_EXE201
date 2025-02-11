using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Application.Interfaces;
using Server.Application.Repositories;
using Server.Domain.Entities;
using Server.Infrastructure.Data;

namespace Server.Infrastructure.Repositories
{
    public class SubCategoryRepository : GenericRepository<SubCategory>, ISubCategoryRepository
    {
        private readonly AppDbContext _dbContext;

        public SubCategoryRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }

        public void DeleteSubCategory(SubCategory SubCategory)
        {
            _dbContext.Remove(SubCategory);
        }

        public async Task<ICollection<SubCategory>> GetSubCategories()
        {
            return await _dbContext.SubCategory.OrderBy(p => p.CreationDate).ToListAsync();
        }

        public async Task<SubCategory> GetSubCategoryById(Guid id)
        {
            return await _dbContext.SubCategory.Include(sc => sc.Category).FirstOrDefaultAsync(sc => sc.Id == id);
        }

        public void UpdateSubCategory(SubCategory SubCategory)
        {
            _dbContext.Update(SubCategory);
        }

        public async Task<SubCategory> GetSubCategoryByName(string name)
        {
            var result = await _dbContext.SubCategory.Where(p => p.SubCategoryName == name).FirstOrDefaultAsync();
            return result;
        }
    }
}
