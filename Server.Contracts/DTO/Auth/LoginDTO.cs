using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.DTO.Auth
{
    public class LoginDTO
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
