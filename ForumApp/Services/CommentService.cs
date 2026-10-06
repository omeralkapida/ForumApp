using ForumApp.Data;
using ForumApp.Entities;
using ForumApp.Features.Comments;
using ForumApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForumApp.Services;

public class CommentService : ICommentService
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CommentService(AppDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<List<CommentDto>> GetByTopicIdAsync(Guid topicId, CancellationToken cancellationToken)
    {
        return await _context.Comments
            .AsNoTracking()
            .Where(x => x.TopicId == topicId && x.ParentCommentId == null)
            .OrderBy(x => x.CreatedAt)
            .Select(x => new CommentDto
            {
                Id = x.Id,
                TopicId = x.TopicId,
                UserId = x.UserId,
                ParentCommentId = x.ParentCommentId,
                Content = x.Content,
                UserName = x.User.UserName!,
                CreatedAt = x.CreatedAt,

                Replies = x.Replies
                    .OrderBy(r => r.CreatedAt)
                    .Select(r => new CommentDto
                    {
                        Id = r.Id,
                        TopicId = r.TopicId,
                        UserId = r.UserId,
                        ParentCommentId = r.ParentCommentId,
                        Content = r.Content,
                        UserName = r.User.UserName!,
                        CreatedAt = r.CreatedAt
                    })
                    .ToList()
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<CommentDto> CreateAsync(Guid topicId, CreateCommentRequest request, CancellationToken cancellationToken)
    {
        var topic = await _context.Topics
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == topicId, cancellationToken);

        if (topic is null)
            throw new Exception("Konu bulunamadı.");

        if (topic.IsClosed)
            throw new Exception("Kapalı bir konuya yorum yapılamaz.");

        if (request.ParentCommentId.HasValue)
        {
            var parentCommentExists = await _context.Comments
                .AsNoTracking()
                .AnyAsync(x => x.Id == request.ParentCommentId.Value && x.TopicId == topicId, cancellationToken);

            if (!parentCommentExists)
                throw new Exception("Cevap verilen yorum bulunamadı veya bu konuya ait değil.");
        }

        var userId = _currentUser.UserId;

        var comment = new Comment
        {
            TopicId = topicId,
            UserId = userId,
            ParentCommentId = request.ParentCommentId,
            Content = request.Content
        };

        _context.Comments.Add(comment);

        await _context.SaveChangesAsync(cancellationToken);

        var userName = await _context.Users
            .Where(x => x.Id == userId)
            .Select(x => x.UserName)
            .FirstAsync(cancellationToken);

        return new CommentDto
        {
            Id = comment.Id,
            TopicId = comment.TopicId,
            UserId = comment.UserId,
            ParentCommentId = comment.ParentCommentId,
            Content = comment.Content,
            UserName = userName!,
            CreatedAt = comment.CreatedAt,
            Replies = []
        };
    }
}