using ForumApp.Data;
using ForumApp.Entities;
using ForumApp.Features.Ratings;
using ForumApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForumApp.Services;

public class RatingService : IRatingService
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public RatingService(AppDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<RatingDto> CreateAsync(Guid commentId, CreateRatingRequest request, CancellationToken cancellationToken)
    {
        if (request.Score < 1 || request.Score > 5)
            throw new Exception("Puan 1 ile 5 arasında olmalıdır.");

        var userId = _currentUser.UserId;

        var comment = await _context.Comments
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == commentId, cancellationToken);

        if (comment is null)
            throw new Exception("Yorum bulunamadı.");

        if (comment.UserId == userId)
            throw new Exception("Kendi yorumunuza puan veremezsiniz.");

        var ratingExists = await _context.CommentRatings
            .AnyAsync(x => x.CommentId == commentId && x.UserId == userId, cancellationToken);

        if (ratingExists)
            throw new Exception("Bu yoruma daha önce puan verdiniz.");

        var rating = new CommentRating
        {
            CommentId = commentId,
            UserId = userId,
            Score = request.Score
        };

        _context.CommentRatings.Add(rating);

        await _context.SaveChangesAsync(cancellationToken);

        return new RatingDto
        {
            Id = rating.Id,
            CommentId = rating.CommentId,
            UserId = rating.UserId,
            Score = rating.Score,
            CreatedAt = rating.CreatedAt
        };
    }
}