using MediatR;
using Microsoft.Extensions.Logging;
using PrReviewAgent.Application.Common.Interfaces;
using PrReviewAgent.Domain.Entities;

namespace PrReviewAgent.Application.Feature.Reviews.Commands.SubmitReview;

public class SubmitReviewCommandHandler(
    IApplicationDbContext dbContext,
    IPrService prService,
    IAiReviewService aiReviewService,
    ILogger<SubmitReviewCommandHandler> logger) : IRequestHandler<SubmitReviewCommand, Guid>
{
    public async Task<Guid> Handle(SubmitReviewCommand request, CancellationToken cancellationToken)
    {
        PullRequestReview review = new(request.PullRequestId);
        dbContext.PullRequestReviews.Add(review);
        await dbContext.SaveChangesAsync(cancellationToken);

        try
        {
            var prDetails = await prService.GetPullRequestDetailsAsync(request.PullRequestId, cancellationToken);
            var aiReviewResult = await aiReviewService.ReviewPullRequestAsync(prDetails, cancellationToken);

            foreach (var finding in aiReviewResult.AiFindings)
                dbContext.ReviewFindings.Add(new ReviewFinding(review.Id, finding.FilePath, finding.Severity, finding.Issue));

            review.UpdateStatus(ReviewStatus.Completed);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "AI review failed for PullRequestId {PullRequestId}", request.PullRequestId);
            review.UpdateStatus(ReviewStatus.Failed);
        }
        finally
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return review.Id;
    }
}
