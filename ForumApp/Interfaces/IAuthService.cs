using ForumApp.Features.Auth;

namespace ForumApp.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> RegisterAsync(
        RegisterRequest request);

    Task<LoginResponse> LoginAsync(
        LoginRequest request);
}