using ForumApp.Features.Ratings;

namespace ForumApp.Interfaces;

public interface IRatingService
{
    Task<RatingDto> CreateAsync(Guid commentId, CreateRatingRequest request, CancellationToken cancellationToken);
}