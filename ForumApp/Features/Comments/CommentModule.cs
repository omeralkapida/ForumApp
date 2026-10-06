using Carter;
using ForumApp.Interfaces;

namespace ForumApp.Features.Comments;

public class CommentModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder group)
    {
        group.MapGroup("/api/comments").WithTags("Comments").RequireRateLimiting("fixed");

        group.MapGet("/topic/{topicId:guid}", GetCommentsByTopic);
        group.MapPost("/topic/{topicId:guid}", CreateComment).RequireAuthorization();
    }

    private static async Task<IResult> GetCommentsByTopic(Guid topicId, ICommentService commentService, CancellationToken cancellationToken)
    {
        var comments = await commentService.GetByTopicIdAsync(topicId, cancellationToken);

        return Results.Ok(comments);
    }

    private static async Task<IResult> CreateComment(Guid topicId, CreateCommentRequest request, ICommentService commentService, CancellationToken cancellationToken)
    {
        var comment = await commentService.CreateAsync(topicId, request, cancellationToken);

        return Results.Created($"/api/topics/{topicId}/comments/{comment.Id}", comment);
    }
}