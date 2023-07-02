using Core.Contracts.Controllers.Auth;
using Core.Entities;
using Riok.Mapperly.Abstractions;

namespace Infrastructure.Mapperly
{
    [Mapper]
    public static partial class UserMapper
    {
        [MapProperty(nameof(RegisterRequest.Username), nameof(User.UserName))]
        public static partial User RegisterRequestToUser(RegisterRequest registerRequest);
    }
}
