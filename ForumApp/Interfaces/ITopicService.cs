using ForumApp.Features.Topic;

public interface ITopicService
{
    Task<List<TopicDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<TopicDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<TopicDto> CreateAsync(CreateTopicRequest request, CancellationToken cancellationToken);
    Task<bool> CloseAsync(Guid id, CancellationToken cancellationToken);
}