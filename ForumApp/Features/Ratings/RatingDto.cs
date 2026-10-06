namespace ForumApp.Features.Ratings;

public record RatingDto
{
    public Guid Id { get; set; }
    public Guid CommentId { get; set; }
    public Guid UserId { get; set; }
    public int Score { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}