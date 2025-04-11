using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTO.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Interfaces
{
    public interface IGoogleService
    {
        Task<string> GetUrlLoginWithGoogle();
        Task<string> GoogleCallback(string code);
        Task<Result<object>> RegisterWithGoogle(GoogleUserRequest request);
        Task<Result<object>> GetOrCreateExternalLoginUser(string provider, string key, string email);
        Task<LoginGoogle> AuthenticateGoogleUserAsync(GoogleUserRequest request);
    }
}
