namespace ForumApp.Features.Comments;

public sealed class CreateCommentRequest
{
    public string Content { get; set; } = null!;
    public Guid? ParentCommentId { get; set; }
}