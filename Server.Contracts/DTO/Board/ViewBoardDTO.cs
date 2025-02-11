using Server.Contracts.DTO.User;
using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.DTO.Board
{
    public class ViewBoardDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public UserDTO? CreatedByUser { get; set; }
        public BoardType Type { get; set; }
        public BoardStatus Status { get; set; }
    }
}
