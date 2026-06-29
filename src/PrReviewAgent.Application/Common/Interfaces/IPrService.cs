using PrReviewAgent.Application.Common.Models;

namespace PrReviewAgent.Application.Common.Interfaces
{
    public interface IPrService
    {
        Task<PrDetails> GetPullRequestDetailsAsync(int pullRequestId, CancellationToken cancellationToken);
    }


}