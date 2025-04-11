using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Server.Application.Interfaces;
using Server.Application.Repositories;
using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTO.Auth;
using Server.Domain.Entities;
using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Services
{
    public class GoogleService : IGoogleService
    {
        private readonly IConfiguration _configuration;
        private readonly IRedisService _redisService;
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;

        public GoogleService(IConfiguration configuration, IRedisService redisService, IUserRepository userRepository, IAuthRepository authRepository)
        {
            _configuration = configuration;
            _redisService = redisService;
            _userRepository = userRepository;
            _authRepository = authRepository;
        }

        public async Task<string> GoogleCallback(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return "Mã code không hợp lệ";
            }
            var client = new HttpClient();
            var values = new Dictionary<string, string>
            {
                { "code", code },
                { "client_id", _configuration["GoogleAPI:ClientId"] },
                { "client_secret", _configuration["GoogleAPI:SecretCode"] },
                { "redirect_uri", _configuration["GoogleAPI:RedirectUri"] },
                { "grant_type", "authorization_code" }
            };
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("https://oauth2.googleapis.com/token", content);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

        public async Task<string> GetUrlLoginWithGoogle()
        {
            var clientId = _configuration["GoogleAPI:ClientId"];
            var redirectUri = _configuration["GoogleAPI:RedirectUri"];

            var url = $"https://accounts.google.com/o/oauth2/auth" +
                      $"?client_id={clientId}" +
                      $"&redirect_uri={redirectUri}" +
                      $"&response_type=code" +
                      $"&scope=email%20profile" +
                      $"&access_type=offline";

            return url;
        }

        public async Task<LoginGoogle> AuthenticateGoogleUserAsync(GoogleUserRequest request)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _configuration["GoogleAPI:ClientId"] }
                });

                var getAccount = await GetOrCreateExternalLoginUser("Google", payload.Subject, payload.Email);
                if (getAccount.Error == 1)
                {
                    return new LoginGoogle()
                    {
                        Code = 1,
                        Error = "Account not exist please register new account",
                        AccessToken = null,
                        RefreshToken = null
                    };
                }
                var token = await GenerateJwtToken((User)getAccount.Data);
                return token;
            }
            catch (InvalidJwtException ex)
            {
                throw new ApplicationException("Invalid Google ID token.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred during Google authentication.", ex);
            }
        }

        public async Task<Result<object>> RegisterWithGoogle(GoogleUserRequest request)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _configuration["GoogleAPI:ClientId"] }
            });

            var getUserToRegister = await GetOrCreateExternalLoginUser("Google", payload.Subject, payload.Email);
            if (getUserToRegister.Error == 0)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Acount already exist",
                    Data = null
                };
            }
            var user = await _userRepository.GetUserByEmail(payload.Email);

            if (user == null)
            {
                user = new User
                {
                    Email = payload.Email,
                    UserName = payload.Email,
                    Balance = 0,
                    PasswordHash = null,
                    Status = StatusEnum.Active,
                    Otp = "",
                    IsStaff = false,
                    RoleId = 2,
                    CreationDate = DateTime.Now,
                    IsVerified = true,
                    Provider = "Google",
                    ProviderKey = payload.Subject,
                    OtpExpiryTime = null
                };
                await _userRepository.AddAsync(user);
            }

            return new Result<object>
            {
                Error = 0,
                Message = "Register Successfully",
                Data = null
            };
        }

        public async Task<Result<object>> GetOrCreateExternalLoginUser(string provider, string key, string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "",
                    Data = null
                };
            }
            if (user.Provider != "Google" && user.ProviderKey != key)
            {
                // Cập nhật thông tin đăng nhập từ Google nếu cần
                user.Provider = "Google";
                user.ProviderKey = key;
                await _userRepository.UpdateAsync(user);
            }
            return new Result<object>
            {
                Error = 0,
                Message = "",
                Data = user
            };
        }

        private async Task<LoginGoogle> GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", user.Id.ToString()), // Ensuring UserId claim is added
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role?.RoleName),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(120), // Token expiration set to 120 minutes
            signingCredentials: creds
            );

            var refreshToken = Guid.NewGuid().ToString();
            await _authRepository.UpdateRefreshToken(user.Id, refreshToken);

            return new LoginGoogle
            {
                Code = 0,
                Error = "Login Successfully",
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken
            };
        }
    }
}
