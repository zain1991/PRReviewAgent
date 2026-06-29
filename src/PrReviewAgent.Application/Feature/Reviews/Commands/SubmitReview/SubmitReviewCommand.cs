using MediatR;

namespace PrReviewAgent.Application.Feature.Reviews.Commands.SubmitReview;

public record SubmitReviewCommand(int PullRequestId) : IRequest<Guid>;