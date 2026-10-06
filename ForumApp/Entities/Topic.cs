using ForumApp.Entities.Coomon;

namespace ForumApp.Entities
{
    public class Topic : BaseEntity
    {
        public Guid UserId { get; set; }

        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;

        public bool IsClosed { get; set; }
        public DateTimeOffset? ClosedAt { get; set; }

        public ApplicationUser User { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; } = [];
    }
}
