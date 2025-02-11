using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.DTO.User
{
    public class SearchServiceUserDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
