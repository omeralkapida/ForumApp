using ForumApp.Entities;
using ForumApp.Features.Auth;
using ForumApp.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ForumApp.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<ApplicationUser> userManager, IJwtService jwtService, IConfiguration configuration)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _configuration = configuration;
    }

    public async Task<LoginResponse> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);

        if (existingUser is not null)
            throw new Exception("Bu e-posta adresi zaten kullanılıyor.");

        var user = new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            Email = request.Email,
            EmailConfirmed = true,
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(x => x.Description));

            throw new Exception(errors);
        }

        return CreateLoginResponse(user);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
            throw new Exception("E-posta adresi veya şifre hatalı.");

        if (!user.IsActive)
            throw new Exception("Kullanıcı hesabı aktif değil.");

        var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!passwordValid)
            throw new Exception("E-posta adresi veya şifre hatalı.");

        user.LastLoginAt = DateTimeOffset.UtcNow;

        await _userManager.UpdateAsync(user);

        return CreateLoginResponse(user);
    }

    private LoginResponse CreateLoginResponse(ApplicationUser user)
    {
        var expirationMinutes = _configuration.GetValue<int>("Jwt:ExpirationMinutes");

        return new LoginResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName!,
            Email = user.Email!,

            AccessToken = _jwtService.GenerateToken(user),

            ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(expirationMinutes)
        };
    }
}