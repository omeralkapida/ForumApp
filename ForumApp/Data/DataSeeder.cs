using ForumApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ForumApp.Data;

public class DataSeeder
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public DataSeeder(
        AppDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _userManager.Users.AnyAsync(cancellationToken))
            return;

        var users = await SeedUsersAsync();

        await SeedTopicsAsync(users, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task<List<ApplicationUser>> SeedUsersAsync()
    {
        var users = new List<ApplicationUser>
        {
            new()
            {
                FirstName = "Ahmet",
                LastName = "Yılmaz",
                UserName = "ahmet",
                Email = "ahmet@test.com",
                EmailConfirmed = true,
                IsActive = true
            },

            new()
            {
                FirstName = "Mehmet",
                LastName = "Kaya",
                UserName = "mehmet",
                Email = "mehmet@test.com",
                EmailConfirmed = true,
                IsActive = true
            },

            new()
            {
                FirstName = "Ayşe",
                LastName = "Demir",
                UserName = "ayse",
                Email = "ayse@test.com",
                EmailConfirmed = true,
                IsActive = true
            }
        };

        foreach (var user in users)
        {
            var result = await _userManager.CreateAsync(
                user,
                "Test123*");

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(x => x.Description));

                throw new Exception(
                    $"{user.Email} oluşturulamadı: {errors}");
            }
        }

        return users;
    }

    private async Task SeedTopicsAsync(
        List<ApplicationUser> users,
        CancellationToken cancellationToken)
    {
        var topicTitles = new[]
        {
            ".NET Dependency Injection nasıl çalışır?",
            "Entity Framework Core performans önerileri",
            "React useEffect ne zaman kullanılmalı?",
            "PostgreSQL index kullanımı",
            "JWT authentication nasıl çalışır?",
            "Carter ile Minimal API kullanımı",
            "ASP.NET Core Middleware nedir?",
            "Repository Pattern gerekli mi?",
            "CancellationToken neden kullanılmalı?",
            "Redis hangi durumlarda kullanılmalı?",
            "Docker öğrenmeye nereden başlamalıyım?",
            "React Redux Toolkit kullanımı",
            "Clean Architecture gerekli mi?",
            "EF Core Include kullanımı",
            "PostgreSQL mi SQL Server mı?",
            "Minimal API avantajları nelerdir?",
            "Global Exception Handling nasıl yapılır?",
            "Soft Delete nasıl uygulanır?",
            "Guid mi int mi kullanılmalı?",
            "API performansı nasıl artırılır?"
        };

        var random = new Random(42);

        foreach (var user in users)
        {
            for (var i = 0; i < topicTitles.Length; i++)
            {
                var isClosed = i % 5 == 0;

                var topic = new Topic
                {
                    UserId = user.Id,

                    Title = topicTitles[i],

                    Content =
                        $"{topicTitles[i]} konusunda bilgi almak istiyorum. " +
                        "Bu konuda tecrübelerinizi ve önerilerinizi paylaşabilir misiniz?",

                    IsClosed = isClosed,

                    ClosedAt = isClosed
                        ? DateTimeOffset.UtcNow
                        : null
                };

                // Diğer iki kullanıcı cevap versin
                var otherUsers = users
                    .Where(x => x.Id != user.Id)
                    .ToList();

                foreach (var commentUser in otherUsers)
                {
                    var comment = new Comment
                    {
                        Topic = topic,
                        UserId = commentUser.Id,

                        Content =
                            $"Bu konuda benim deneyimime göre " +
                            $"{topicTitles[i]} için birkaç farklı yaklaşım kullanılabilir."
                    };

                    topic.Comments.Add(comment);

                    // Topic sahibi cevaba reply atsın
                    var reply = new Comment
                    {
                        Topic = topic,
                        UserId = user.Id,
                        ParentComment = comment,

                        Content =
                            "Cevabınız için teşekkür ederim. " +
                            "Bu yaklaşımı biraz daha açıklayabilir misiniz?"
                    };

                    topic.Comments.Add(reply);

                    // Topic sahibi başkasının yorumuna puan versin
                    comment.Ratings.Add(new CommentRating
                    {
                        UserId = user.Id,
                        Score = random.Next(3, 6)
                    });

                    // Diğer kullanıcı reply'a puan versin
                    reply.Ratings.Add(new CommentRating
                    {
                        UserId = commentUser.Id,
                        Score = random.Next(3, 6)
                    });
                }

                _context.Topics.Add(topic);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}