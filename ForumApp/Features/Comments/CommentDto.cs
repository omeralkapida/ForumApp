namespace ForumApp.Features.Comments;

public record CommentDto
{
    public Guid Id { get; set; }
    public Guid TopicId { get; set; }
    public Guid UserId { get; set; }
    public Guid? ParentCommentId { get; set; }
    public string Content { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public List<CommentDto> Replies { get; set; } = [];
}