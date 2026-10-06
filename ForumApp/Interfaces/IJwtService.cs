using ForumApp.Entities;

namespace ForumApp.Interfaces;

public interface IJwtService
{
    string GenerateToken(ApplicationUser user);
}