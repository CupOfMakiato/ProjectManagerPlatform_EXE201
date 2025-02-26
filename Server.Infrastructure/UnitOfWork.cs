﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Application;
using Server.Application.Repositories;
using Server.Infrastructure.Data;
using Server.Infrastructure.Repositories;

namespace Server.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBoardRepository _boardRepository;
        private readonly IColumnRepository _columnRepository;



        public UnitOfWork(AppDbContext dbContext, ISubCategoryRepository subCategoryRepository, ICategoryRepository categoryRepository,
            IAuthRepository authRepository, IUserRepository userRepository, IBoardRepository boardRepository, IColumnRepository columnRepository)
        {
            _dbContext = dbContext;
            _subCategoryRepository = subCategoryRepository;
            _categoryRepository = categoryRepository;
            _authRepository = authRepository;
            _userRepository = userRepository;
            _boardRepository = boardRepository;
            _columnRepository = columnRepository;
        }

        public ICategoryRepository categoryRepository => _categoryRepository;

        public ISubCategoryRepository subCategoryRepository => _subCategoryRepository;
        public IAuthRepository authRepository => _authRepository;
        public IUserRepository userRepository => _userRepository;
        public IBoardRepository boardRepository => _boardRepository;
        public IColumnRepository columnRepository => _columnRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
