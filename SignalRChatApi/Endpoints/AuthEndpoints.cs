using Core.Contracts.Controllers.Auth;
using Core.Mediator.Commands.Auth;
using Core.Mediator.Queries.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SignalRChatApi.Endpoints
{
    public static class AuthEndpoints
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/mini/auth/login", Login);
            app.MapPost("api/mini/auth/register", Register);
            app.MapGet("api/mini/auth/confirmEmail", ConfirmEmail);
            app.MapGet("api/mini/auth/resetPassword", ResetPassword);
            app.MapGet("api/mini/auth/confirmResetPassword", ConfirmResetPassword);
            app.MapGet("api/mini/auth/userData", GetUserData);
            app.MapGet("api/mini/auth/protected", Protected);
            app.MapGet("api/mini/auth/adminProtected", AdminProtected);

            return app;
        }

        public static async Task<IResult> Login(IMediator mediator, LoginRequest loginRequest)
        {
            var result = await mediator.Send(new LoginCommand(loginRequest));

            if (result is not null)
            {
                return Results.Ok(result);
            }

            return Results.BadRequest();
        }

        public static async Task<IResult> Register(IMediator mediator, RegisterRequest registerRequest)
        {
            var result = await mediator.Send(new RegisterCommand(registerRequest));

            if (!result.Any())
            {
                return Results.Ok();
            }

            return Results.BadRequest(new Failed(result));
        }

        public static async Task<IResult> ConfirmEmail(IMediator mediator, [AsParameters] ConfirmEmailRequest confirmEmailRequest)
        {
            var errors = await mediator.Send(new ConfirmEmailCommand(confirmEmailRequest));

            if (!errors.Any())
            {
                return Results.Ok();
            }

            return Results.BadRequest(new Failed(errors));
        }

        public static async Task<IResult> ResetPassword(IMediator mediator, [AsParameters] ResetPasswordRequest resetPasswordRequest)
        {
            var result = await mediator.Send(new ResetPasswordCommand(resetPasswordRequest));

            if (result)
            {
                return Results.Ok();
            }

            return Results.NotFound();
        }

        public static async Task<IResult> ConfirmResetPassword(IMediator mediator, [AsParameters] ConfirmResetPasswordRequest confirmResetPasswordRequest)
        {
            var errors = await mediator.Send(new ConfirmResetPasswordCommand(confirmResetPasswordRequest));

            if (!errors.Any())
            {
                return Results.Ok();
            }

            return Results.BadRequest(new Failed(errors));
        }

        public static async Task<IResult> GetUserData(IMediator mediator, ClaimsPrincipal user)
        {
            var data = await mediator.Send(new GetUserDataQuery(user));

            if (data is not null)
            {
                return Results.Ok(data);
            }

            return Results.NotFound();
        }

        [Authorize]
        public static IResult Protected()
        {
            return Results.Ok();
        }

        [Authorize(Roles = "Admin")]
        public static IResult AdminProtected()
        {
            return Results.Ok();
        }
    }
}
