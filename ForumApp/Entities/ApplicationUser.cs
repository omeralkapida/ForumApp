using Microsoft.AspNetCore.Identity;

namespace ForumApp.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public string? ProfileImageUrl { get; set; }
        public string? Bio { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? LastLoginAt { get; set; }

        public ICollection<Topic> Topics { get; set; } = [];
        public ICollection<Comment> Comments { get; set; } = [];
        public ICollection<CommentRating> CommentRatings { get; set; } = [];
    }
}
