using MediatR;

namespace PrReviewAgent.Application.Feature.Reviews.Queries.GetReviewStatus;

public record ReviewStatusQuery(Guid Id) : IRequest<ReviewStatusResult>;