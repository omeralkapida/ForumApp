namespace ForumApp.Features.Auth;

public record LoginResponse
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string AccessToken { get; set; } = null!;
    public DateTimeOffset ExpiresAt { get; set; }
}