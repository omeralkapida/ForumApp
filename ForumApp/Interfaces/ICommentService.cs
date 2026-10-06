using ForumApp.Features.Comments;

namespace ForumApp.Interfaces;

public interface ICommentService
{
    Task<List<CommentDto>> GetByTopicIdAsync(Guid topicId, CancellationToken cancellationToken);
    Task<CommentDto> CreateAsync(Guid topicId, CreateCommentRequest request, CancellationToken cancellationToken);
}