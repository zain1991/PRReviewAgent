using Microsoft.EntityFrameworkCore;
using PrReviewAgent.Domain.Entities;

namespace PrReviewAgent.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<PullRequestReview> PullRequestReviews { get; }
        DbSet<ReviewFinding> ReviewFindings { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
