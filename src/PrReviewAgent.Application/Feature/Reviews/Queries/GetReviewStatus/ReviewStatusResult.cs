using PrReviewAgent.Domain.Entities;

public record ReviewStatusResult
{
    public Guid Id { get; init; }
    public int PullRequestId { get; init; }
    public ReviewStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
}