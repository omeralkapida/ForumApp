using ForumApp.Entities.Coomon;

namespace ForumApp.Entities
{
    public class CommentRating : BaseEntity
    {
        public Guid CommentId { get; set; }
        public Guid UserId { get; set; }

        public int Score { get; set; }

        public Comment Comment { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}
