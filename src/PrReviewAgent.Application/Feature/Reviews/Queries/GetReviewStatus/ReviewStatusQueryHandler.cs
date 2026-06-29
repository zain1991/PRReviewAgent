using MediatR;
using Microsoft.EntityFrameworkCore;
using PrReviewAgent.Application.Common.Interfaces;

namespace PrReviewAgent.Application.Feature.Reviews.Queries.GetReviewStatus;

public class ReviewStatusQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<ReviewStatusQuery, ReviewStatusResult>
{
    public async Task<ReviewStatusResult> Handle(ReviewStatusQuery request, CancellationToken cancellationToken)
    {
        var review = await dbContext.PullRequestReviews
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (review is null)
            throw new KeyNotFoundException($"Review {request.Id} not found.");

        return new ReviewStatusResult
        {
            Id = review.Id,
            PullRequestId = review.PullRequestId,
            Status = review.Status,
            CreatedAt = review.CreatedAt
        };
    }
}
