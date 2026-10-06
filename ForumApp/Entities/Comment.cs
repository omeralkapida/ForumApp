using ForumApp.Entities.Coomon;

namespace ForumApp.Entities
{
    public class Comment:BaseEntity
    {
        public Guid TopicId { get; set; }
        public Guid UserId { get; set; }

        public Guid? ParentCommentId { get; set; }

        public string Content { get; set; } = null!;

        public Topic Topic { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public Comment? ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; } = [];

        public ICollection<CommentRating> Ratings { get; set; } = [];
    }
}
