using Carter;
using ForumApp.Interfaces;

namespace ForumApp.Features.Ratings;

public class RatingModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder group)
    {
        group.MapGroup("/api/comments").WithTags("Ratings").RequireRateLimiting("fixed");

        group.MapPost("/{commentId:guid}/rating", Create).RequireAuthorization();
    }

    private static async Task<IResult> Create(Guid commentId, CreateRatingRequest request, IRatingService ratingService, CancellationToken cancellationToken)
    {
        var rating = await ratingService.CreateAsync(commentId, request, cancellationToken);

        return Results.Ok(rating);
    }
}