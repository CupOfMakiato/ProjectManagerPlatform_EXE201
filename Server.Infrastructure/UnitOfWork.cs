using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Application;
using Server.Application.Repositories;
using Server.Domain.Entities;
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
        private readonly INotificationRepository _notificationRepository;
        private readonly ISubcriptionRepository _subcriptionRepository;





        public UnitOfWork(AppDbContext dbContext, ISubCategoryRepository subCategoryRepository, ICategoryRepository categoryRepository,
            IAuthRepository authRepository, IUserRepository userRepository, IBoardRepository boardRepository, ICardRepository cardRepository,
            IColumnRepository columnRepository, IAttachmentRepository attachmentRepository, INotificationRepository notificationRepository,
            ISubcriptionRepository subcriptionRepository)
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
            _notificationRepository = notificationRepository;
            _subcriptionRepository = subcriptionRepository;

        }

        public ICategoryRepository categoryRepository => _categoryRepository;

        public ISubCategoryRepository subCategoryRepository => _subCategoryRepository;
        public IAuthRepository authRepository => _authRepository;
        public IUserRepository userRepository => _userRepository;
        public IBoardRepository boardRepository => _boardRepository;
        public ICardRepository cardRepository => _cardRepository;
        public IColumnRepository columnRepository => _columnRepository;
        public IAttachmentRepository attachmentRepository => _attachmentRepository;
        public INotificationRepository notificationRepository => _notificationRepository;
        public ISubcriptionRepository subcriptionRepository => _subcriptionRepository;


        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
