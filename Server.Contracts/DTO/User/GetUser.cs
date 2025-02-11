using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.DTO.User
{
    public class GetUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Status { get; set; }
        public bool IsVerify { get; set; }
        public string Role { get; set; }
        public double Balance { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
