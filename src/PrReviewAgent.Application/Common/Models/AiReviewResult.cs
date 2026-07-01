namespace PrReviewAgent.Application.Common.Models
{
    public record AiReviewResult(
    IReadOnlyList<AiFinding> AiFindings);
}