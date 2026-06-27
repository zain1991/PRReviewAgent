using Microsoft.EntityFrameworkCore;
using PrReviewAgent.Application.Common.Interfaces;
using PrReviewAgent.Domain.Entities;

namespace PRReviewAgent.Infrastructure.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IApplicationDbContext
    {
        public DbSet<PullRequestReview> PullRequestReviews { get; set; }
        public DbSet<ReviewFinding> ReviewFindings { get; set; }
    }
}