using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTO.SubCategory;
using Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Interfaces
{
    public interface ISubCategoryService
    {
        public Task<Result<List<ViewSubCategoryDTO>>> GetSubs();
        public Task<Result<object>> GetById(Guid Id);
        public Task<Result<SubCategory>> GetSubByIdWithoutMap(Guid Id);
        public Task<Result<object>> Create(SubCategory subCategory);
        public Task<Result<object>> Update(Guid Id, string Name, int Status);
        public Task<SubCategory> GetByName(string Name);
        public Task<Result<object>> AddSubCategoryToCategory(SubCategory subCategory, Guid CategoryId);
        public Task<Result<object>> Delete(Guid Id);
    }
}
