using PrReviewAgent.Application.Common.Models;

namespace PrReviewAgent.Application.Common.Interfaces
{
    public interface IAiReviewService
    {
        Task<AiReviewResult> ReviewPullRequestAsync(PrDetails prDetails, CancellationToken cancellationToken);
    }


}