using MediatR;
using PrReviewAgent.Application.Feature.Reviews.Commands.SubmitReview;
using PrReviewAgent.Application.Feature.Reviews.Queries.GetReviewStatus;

namespace PrReviewAgent.Api.Endpoints;

public static class ReviewsEndpoints
{
    public static IEndpointRouteBuilder MapReviewEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/reviews", async (SubmitReviewCommand command, ISender sender) =>
        {
            var id = await sender.Send(command);
            return Results.Created($"/api/reviews/{id}", id);
        });

        app.MapGet("/api/reviews/{id:guid}", async (Guid id, ISender sender) =>
        {
            try
            {
                var result = await sender.Send(new ReviewStatusQuery(id));
                return Results.Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        });

        return app;
    }
}
