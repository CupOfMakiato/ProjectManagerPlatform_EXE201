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
        private readonly ICardRepository _cardRepository;
        private readonly IColumnRepository _columnRepository;
        private readonly IAttachmentRepository _attachmentRepository;




        public UnitOfWork(AppDbContext dbContext, ISubCategoryRepository subCategoryRepository, ICategoryRepository categoryRepository,
            IAuthRepository authRepository, IUserRepository userRepository, IBoardRepository boardRepository, ICardRepository cardRepository,
            IColumnRepository columnRepository, IAttachmentRepository attachmentRepository)
        {
            _dbContext = dbContext;
            _subCategoryRepository = subCategoryRepository;
            _categoryRepository = categoryRepository;
            _authRepository = authRepository;
            _userRepository = userRepository;
            _boardRepository = boardRepository;
            _cardRepository = cardRepository;
            _columnRepository = columnRepository;
            _attachmentRepository = attachmentRepository;
        }

        public ICategoryRepository categoryRepository => _categoryRepository;

        public ISubCategoryRepository subCategoryRepository => _subCategoryRepository;
        public IAuthRepository authRepository => _authRepository;
        public IUserRepository userRepository => _userRepository;
        public IBoardRepository boardRepository => _boardRepository;
        public ICardRepository cardRepository => _cardRepository;
        public IColumnRepository columnRepository => _columnRepository;
        public IAttachmentRepository attachmentRepository => _attachmentRepository;



        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
