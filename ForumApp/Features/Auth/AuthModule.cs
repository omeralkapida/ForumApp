using Carter;
using ForumApp.Features.Auth;
using ForumApp.Interfaces;

namespace ForumApp.Features.Auth;

public class AuthModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder group)
    {
        group.MapGroup("/api/auth").WithTags("Auth").RequireRateLimiting("fixed");

        group.MapPost("/register", Register);
        group.MapPost("/login", Login);
    }

    private static async Task<IResult> Register(RegisterRequest request, IAuthService authService)
    {
        var response = await authService.RegisterAsync(request);
        return Results.Ok(response);
    }

    private static async Task<IResult> Login(LoginRequest request, IAuthService authService)
    {
        var response = await authService.LoginAsync(request);
        return Results.Ok(response);
    }
}