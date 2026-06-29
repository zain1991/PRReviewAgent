namespace PrReviewAgent.Application.Common.Models
{
    public record PrDetails(
    int PullRequestId,
    string Title,
    string Author,
    IReadOnlyList<string> FilesChanged);
}