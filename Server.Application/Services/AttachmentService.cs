using AutoMapper;
using Microsoft.AspNetCore.Http;
using Server.Application.Interfaces;
using Server.Application.Repositories;
using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTO.Attachment;
using Server.Contracts.DTO.Card;
using Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IAttachmentRepository _attachmentRepository;
        public AttachmentService(IUnitOfWork unitOfWork, IMapper mapper,
            ICloudinaryService cloudinaryService, IHttpContextAccessor contextAccessor,
            IEmailService emailService, IUserService userService,
            IAttachmentRepository attachmentRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = contextAccessor;
            _emailService = emailService;
            _userService = userService;
            _attachmentRepository = attachmentRepository;
        }

        public async Task<Result<object>> ChangeAttachmentName(ChangeAttachmentNameDTO changeAttachmentNameDTO)
        {
            var attachment = await _unitOfWork.attachmentRepository.GetAttachmentById(changeAttachmentNameDTO.Id);
            if (attachment == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Attachment not found",
                    Data = null
                };
            }

            attachment.FileName = changeAttachmentNameDTO.FileName;

            _unitOfWork.attachmentRepository.Update(attachment);
            var result = await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Change Attachment Name successfully" : "Change attachment name failed",
                Data = null
            };
        }

        public async Task<Result<object>> MakeCover(Guid attachmentId)
        {
            var attachment = await _attachmentRepository.GetByIdAsync(attachmentId);
            if (attachment == null)
            {
                return new Result<object> { Error = 1, Message = "Attachment not found" };
            }

            // Remove cover from existing attachments in the same card
            var cardAttachments = await _attachmentRepository.GetAttachmentsByCardId(attachment.CardId);
            foreach (var att in cardAttachments)
            {
                att.IsCover = (att.Id == attachmentId);
                _attachmentRepository.Update(att);
            }

            return new Result<object>
            {
                Error = 0,
                Message = "Attachment is now the cover",
                Data = attachment
            };
        }
    }
}
