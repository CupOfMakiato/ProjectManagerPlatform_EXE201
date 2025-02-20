using Server.Contracts.DTO.User;
using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.DTO.Column
{
    public class ViewColumnDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int CollumnPosition { get; set; }
        public ColumnStatus? Status { get; set; }
        public Guid BoardId { get; set; }
        public UserDTO? CreatedByUser { get; set; }
    }
}
