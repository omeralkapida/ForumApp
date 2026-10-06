namespace ForumApp.Features.Topic
{
    public record TopicDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public bool IsClosed { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;

        public int CommentCount { get; set; }
    }
}
