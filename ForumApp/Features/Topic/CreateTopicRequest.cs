namespace ForumApp.Features.Topic
{
    public sealed class CreateTopicRequest
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
    }
}
