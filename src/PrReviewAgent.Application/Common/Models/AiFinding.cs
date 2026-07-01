namespace PrReviewAgent.Application.Common.Models
{
    public record AiFinding(
    string FilePath,
    string Severity,
    string Issue);
}