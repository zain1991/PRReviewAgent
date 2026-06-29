using MediatR;
using PrReviewAgent.Application.Common.Interfaces;
using PrReviewAgent.Domain.Entities;

namespace PrReviewAgent.Application.Feature.Reviews.Commands.SubmitReview;
public class SubmitReviewCommandHandler : IRequestHandler<SubmitReviewCommand, Guid>
{
    private readonly IApplicationDbContext _dbContext;
    public SubmitReviewCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Handle(SubmitReviewCommand request, CancellationToken cancellationToken)
    {
        PullRequestReview review = new PullRequestReview(request.PullRequestId);
        _dbContext.PullRequestReviews.Add(review);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return await Task.FromResult(review.Id);
    }
}
    