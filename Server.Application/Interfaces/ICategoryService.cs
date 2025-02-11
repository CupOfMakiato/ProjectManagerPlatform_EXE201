using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTO.Category;
using Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Interfaces
{
    public interface ICategoryService
    {
        public Task<Result<List<ViewCategoryDTO>>> GetCategory(); 
        public Task<Result<ViewCategoryDTO>> GetCategoryIdWithMap(Guid Id);
        public Task<Category?> GetCategoryIdWithoutMap(Guid Id);
        public Task<Result<object>> UpdateCateWithNameAndStatus(Category category, string cateName, int Status);
        public Task<Category> GetCategoryByName(string name);
        public Task<Result<object>> Create(Category category);
        public Task<Result<object>> Delete(Guid Id);
    }
}
