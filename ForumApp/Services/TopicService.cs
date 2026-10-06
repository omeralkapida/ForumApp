using ForumApp.Data;
using ForumApp.Entities;
using ForumApp.Features.Topic;
using ForumApp.Interfaces;
using Microsoft.EntityFrameworkCore;

public class TopicService : ITopicService
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public TopicService(AppDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<List<TopicDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Topics
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new TopicDto
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                IsClosed = x.IsClosed,
                CreatedAt = x.CreatedAt,
                UserId = x.UserId,
                UserName = x.User.UserName!,
                CommentCount = x.Comments.Count()
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<TopicDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Topics
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new TopicDto
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                IsClosed = x.IsClosed,
                CreatedAt = x.CreatedAt,
                UserId = x.UserId,
                UserName = x.User.UserName!,
                CommentCount = x.Comments.Count()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TopicDto> CreateAsync(CreateTopicRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var topic = new Topic
        {
            UserId = userId,
            Title = request.Title,
            Content = request.Content
        };

        _context.Topics.Add(topic);

        await _context.SaveChangesAsync(cancellationToken);

        var userName = await _context.Users
            .Where(x => x.Id == userId)
            .Select(x => x.UserName)
            .FirstAsync(cancellationToken);

        return new TopicDto
        {
            Id = topic.Id,
            Title = topic.Title,
            Content = topic.Content,
            IsClosed = topic.IsClosed,
            CreatedAt = topic.CreatedAt,
            UserId = topic.UserId,
            UserName = userName!,
            CommentCount = 0
        };
    }

    public async Task<bool> CloseAsync(Guid id, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var topic = await _context.Topics.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (topic is null)
            return false;

        if (topic.UserId != userId)
            throw new UnauthorizedAccessException("Bu konuyu kapatma yetkiniz bulunmuyor.");

        if (topic.IsClosed)
            return true;

        topic.IsClosed = true;
        topic.ClosedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}