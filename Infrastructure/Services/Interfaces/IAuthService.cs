using Core.Contracts.Controllers.Auth;
using Core.Models;
using System.Security.Claims;

namespace Infrastructure.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Jwt?> Login(LoginRequest login);
        Task<IEnumerable<string>> Register(RegisterRequest register);
        Task<IEnumerable<string>> ConfirmEmail(ConfirmEmailRequest confirmEmailRequest);
        Task<bool> ResetPassword(ResetPasswordRequest resetPasswordRequest);
        Task<IEnumerable<string>> ConfirmResetPassword(ConfirmResetPasswordRequest confirmResetPasswordRequest);
        Task<UserResponse?> GetUserData(ClaimsPrincipal user);
    }
}
