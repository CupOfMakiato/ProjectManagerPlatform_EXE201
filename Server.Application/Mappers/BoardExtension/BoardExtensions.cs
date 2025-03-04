using Server.Application.Mappers.UserExtension;
using Server.Contracts.Abstractions.RequestAndResponse.Board;
using Server.Contracts.DTO.Board;
using Server.Domain.Entities;
using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Mappers.BoardExtension
{
    public static class BoardExtensions
    {
        public static ViewBoardDTO ToViewBoardDTO(this Board board)
        {
            return new ViewBoardDTO
            {
                Id = board.Id,
                Title = board.Title,
                Description = board.Description,
                //ThumbNail = board.ThumbNail, //add later
                Type = (Domain.Enums.BoardType)board.Type,
                Status = (Domain.Enums.BoardStatus)board.Status,
                CreatedByUser = board.BoardCreatedByUser.ToUserDTO()

            };
        }
        public static Board ToBoard(this AddBoardDTO addBoardDTO)
        {
            return new Board
            {
                Id = addBoardDTO.Id,
                Title = addBoardDTO.Title,
                Description = addBoardDTO.Description,
                //ThumbNail = board.ThumbNail, //add later
                Type = addBoardDTO.Type,
                //Status = addBoardDTO.Status, //always Open when added
                //BoardCreatedBy = addBoardDTO.UserId,
                CreatedBy = addBoardDTO.UserId,

            };
        }
        public static AddBoardDTO ToAddBoardDTO(this AddNewBoardRequest addNewBoardRequest)
        {
            return new AddBoardDTO
            {
                Id = (Guid)addNewBoardRequest.Id,
                UserId = addNewBoardRequest.UserId,
                Title = addNewBoardRequest.Title,
                Description = addNewBoardRequest.Description,
                //ThumbNail = board.ThumbNail, //add later
                Type = addNewBoardRequest.Type, 
                Status = BoardStatus.Open,

            };
        }
        public static UpdateBoardDTO ToUpdateBoardDTO(this UpdateBoardRequest updateBoardRequest)
        {
            return new UpdateBoardDTO
            {
                Id = (Guid)updateBoardRequest.Id,
                Title = updateBoardRequest.Title,
                Description = updateBoardRequest.Description,
            };
        }
        public static ChangeBoardNameDTO ToChangeBoardNameDTO(this ChangeBoardNameRequest changeBoardNameRequest)
        {
            return new ChangeBoardNameDTO
            {
                Id = (Guid)changeBoardNameRequest.Id,
                Title = changeBoardNameRequest.Title,
            };
        }
    }
}
