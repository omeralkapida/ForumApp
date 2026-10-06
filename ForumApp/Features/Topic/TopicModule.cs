using Carter;
using ForumApp.Features.Topic;
using System.Security.Claims;

public class TopicModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder group)
    {
        group = group.MapGroup("/api/topics").WithTags("Topics").RequireRateLimiting("fixed");

        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById);
        group.MapPost("/", Create).RequireAuthorization();
        group.MapPut("/{id:guid}/close", Close).RequireAuthorization();
    }

    private static async Task<IResult> GetAll(ITopicService topicService, CancellationToken cancellationToken)
    {
        var topics = await topicService.GetAllAsync(cancellationToken);

        return Results.Ok(topics);
    }

    private static async Task<IResult> GetById(Guid id, ITopicService topicService, CancellationToken cancellationToken)
    {
        var topic = await topicService.GetByIdAsync(id, cancellationToken);

        if (topic is null)
            return Results.NotFound();

        return Results.Ok(topic);
    }

    private static async Task<IResult> Create(CreateTopicRequest request, ITopicService topicService, CancellationToken cancellationToken)
    {
        var topic = await topicService.CreateAsync(request, cancellationToken);

        return Results.Created($"/api/topics/{topic.Id}", topic);
    }

    private static async Task<IResult> Close(Guid id, ITopicService topicService, CancellationToken cancellationToken)
    {
        var result = await topicService.CloseAsync(id, cancellationToken);

        if (!result)
            return Results.NotFound();

        return Results.NoContent();
    }
}