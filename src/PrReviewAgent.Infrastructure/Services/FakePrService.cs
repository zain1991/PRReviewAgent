using PrReviewAgent.Application.Common.Interfaces;
using PrReviewAgent.Application.Common.Models;

namespace PrReviewAgent.Infrastructure.Services
{
    public class FakePrService : IPrService
    {
        public async Task<PrDetails> GetPullRequestDetailsAsync(int pullRequestId, CancellationToken cancellationToken)
        {

            return await Task.FromResult(new PrDetails(
            PullRequestId: pullRequestId,
            Title: $"Fake PR Title for {pullRequestId}",
            Author: $"Fake Author for {pullRequestId}",
            FilesChanged: [$"Fake File 1 for {pullRequestId}", $"Fake File 2 for {pullRequestId}"]
        ));
        }
    }
}