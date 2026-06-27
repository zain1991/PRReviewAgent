

namespace PrReviewAgent.Domain.Entities
{
    public class ReviewFinding
    {
        private ReviewFinding() { }

        public ReviewFinding(Guid reviewId, string filePath, string severity, string issue)
        {
            Id = Guid.NewGuid();
            ReviewId = reviewId;
            FilePath = filePath;
            Severity = severity;
            Issue = issue;
        }

        public Guid Id { get; private set; }

        public Guid ReviewId { get; private set; }

        public string FilePath { get; private set; } = default!;

        public string Severity { get; private set; } = default!;

        public string Issue { get; private set; } = default!;
    }
}
