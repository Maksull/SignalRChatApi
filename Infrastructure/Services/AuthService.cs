using Core.Contracts.Controllers.Auth;
using Core.Entities;
using Core.Models;
using Infrastructure.Mapperly;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web;

namespace Infrastructure.Services
{
    public sealed class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly UserManager<User> _userManager;

        public AuthService(IConfiguration configuration, IEmailService emailService, UserManager<User> userManager)
        {
            _configuration = configuration;
            _emailService = emailService;
            _userManager = userManager;
        }

        public async Task<Jwt?> Login(LoginRequest login)
        {
            var user = await _userManager.FindByNameAsync(login.Username);

            if (user is null)
            {
                return null;
            }
            if (!await _userManager.CheckPasswordAsync(user, login.Password))
            {
                return null;
            }

            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!isEmailConfirmed)
            {
                return null;
            }

            string token = await CreateToken(user);
            RefreshToken refreshToken = GenerateRefreshToken();

            await UpdateUserRefreshTokenAsync(user, refreshToken);

            return new(token, refreshToken);
        }

        public async Task<IEnumerable<string>> Register(RegisterRequest register)
        {
            User user = UserMapper.RegisterRequestToUser(register);

            var result = await _userManager.CreateAsync(user, register.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var confirmationLink = $"{_configuration["SignalRChatAngular:Url"]}/login/confirmEmail/{user.Id}/{HttpUtility.UrlEncode(token)}";

                string emailBody = $"Please confirm your email by clicking the link below:<br/><br/><a href=\"{confirmationLink}\">Confirm Email</a>";

                await _emailService.SendMessageAsync(new Core.Contracts.Services.EmailService.EmailRequest(user.Email!, "Email confirmation", emailBody));

                return Enumerable.Empty<string>();
            }

            List<string> errors = new();
            foreach (var error in result.Errors)
            {
                errors.Add(error.Description);
            }

            return errors;
        }

        public async Task<IEnumerable<string>> ConfirmEmail(ConfirmEmailRequest confirmEmailRequest)
        {
            List<string> errors = new();
            var user = await _userManager.FindByIdAsync(confirmEmailRequest.UserId);

            if (user is null)
            {
                errors.Add("User does not exist");

                return errors;
            }

            var result = await _userManager.ConfirmEmailAsync(user, confirmEmailRequest.Token);

            if (result.Succeeded)
            {
                return Enumerable.Empty<string>();
            }

            foreach (var error in result.Errors)
            {
                errors.Add(error.Description);
            }

            return errors;
        }

        public async Task<bool> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            User? user = await _userManager.FindByNameAsync(resetPasswordRequest.Username);

            if (user is null)
            {
                return false;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var confirmationLink = $"{_configuration["SignalRChatAngular:Url"]}/login/resetPassword/{user.Id}/{HttpUtility.UrlEncode(token)}";

            string emailBody = $"Please confirm your password by clicking the link below:<br/><br/><a href=\"{confirmationLink}\">Reset Password</a>";

            await _emailService.SendMessageAsync(new Core.Contracts.Services.EmailService.EmailRequest(user.Email!, "Password reset", emailBody));

            return true;
        }

        public async Task<IEnumerable<string>> ConfirmResetPassword(ConfirmResetPasswordRequest confirmResetPasswordRequest)
        {
            List<string> errors = new();
            var user = await _userManager.FindByIdAsync(confirmResetPasswordRequest.UserId);

            if (user is null)
            {
                errors.Add("User does not exist");

                return errors;
            }

            var result = await _userManager.ResetPasswordAsync(user, confirmResetPasswordRequest.Token, confirmResetPasswordRequest.NewPassword);

            if (result.Succeeded)
            {
                return Enumerable.Empty<string>();
            }

            foreach (var error in result.Errors)
            {
                errors.Add(error.Description);
            }

            return errors;
        }

        public async Task<UserResponse?> GetUserData(ClaimsPrincipal user)
        {
            var userId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value.ToString()!;
            var u = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (u is not null)
            {
                UserResponse result = new(FirstName: u.FirstName, LastName: u.LastName, Username: u.UserName!, Email: u.Email!, PhoneNumber: u.PhoneNumber!);

                return result;
            }

            return null;
        }


        private async Task<string> CreateToken(User user)
        {
            var roles = await _userManager.GetRolesAsync(user) ?? Array.Empty<string>();

            List<Claim> claims = new()
            {
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.NameIdentifier, user.Id),
            };

            foreach (var role in roles)
            {
                claims.Add(new(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings:SecurityKey").Value!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var expirationDateTime = DateTime.UtcNow.AddMinutes(double.Parse(_configuration.GetSection("JwtSettings:ExpiresInMinutes").Value!));

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expirationDateTime,
                signingCredentials: credentials
                );

            string jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        private RefreshToken GenerateRefreshToken()
        {
            RefreshToken refreshToken = new(
                Token: Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expired: DateTime.UtcNow.AddHours(double.Parse(_configuration.GetSection("RefreshToken:ExpiresInHours").Value!)));

            return refreshToken;
        }
        private async Task UpdateUserRefreshTokenAsync(User user, RefreshToken refreshToken)
        {
            user.RefreshToken = refreshToken.Token;
            user.RefreshTokenExpired = refreshToken.Expired;

            await _userManager.UpdateAsync(user);
        }
    }
}
