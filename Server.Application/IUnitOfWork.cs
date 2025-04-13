using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Application.Repositories;

namespace Server.Application
{
    public interface IUnitOfWork
    {

        IUserRepository userRepository { get; }
        ICategoryRepository categoryRepository { get; }
        ISubCategoryRepository subCategoryRepository { get; }
        IAuthRepository authRepository { get; }
        IBoardRepository boardRepository { get; }
        ICardRepository cardRepository { get; }
        IColumnRepository columnRepository { get; }
        IAttachmentRepository attachmentRepository { get; }
        INotificationRepository notificationRepository { get; }
        ISubcriptionRepository subcriptionRepository { get; }
        ISubcribeRepository subcribeRepository { get; }
        IPaymentRepository paymentRepository { get; }

        public Task<int> SaveChangeAsync();
    }
}
