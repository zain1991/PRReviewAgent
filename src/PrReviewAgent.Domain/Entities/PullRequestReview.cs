

namespace PrReviewAgent.Domain.Entities
{
    public class PullRequestReview
    {
        private PullRequestReview() { }

        public PullRequestReview(int pullRequestId)
        {
            Id = Guid.NewGuid();
            PullRequestId = pullRequestId;
            Status = ReviewStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateStatus(ReviewStatus status)
        {
            Status = status;
        }

        public Guid Id { get; private set; }

        public int PullRequestId { get; private set; }

        public ReviewStatus Status { get; private set; }

        public DateTime CreatedAt { get; private set; }
    }
}
