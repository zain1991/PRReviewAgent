using System.Text.Json;
using OllamaSharp;
using Microsoft.Extensions.AI;
using PrReviewAgent.Application.Common.Interfaces;
using PrReviewAgent.Application.Common.Models;

namespace PrReviewAgent.Infrastructure.Services
{
    public class OllamaReviewService(IChatClient chatClient) : IAiReviewService
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<AiReviewResult> ReviewPullRequestAsync(PrDetails prDetails, CancellationToken cancellationToken)
        {
            var messages = new List<ChatMessage>
            {
                new(ChatRole.System, """
                    You are a senior code reviewer. Analyze the pull request details provided and return findings as JSON only.
                    Do not include any explanation, markdown, or text outside the JSON object.
                    Severity must be one of: Critical, Major, Minor, Info.
                    """),
                new(ChatRole.User, BuildPrompt(prDetails))
            };

            var options = new ChatOptions
            {
                ResponseFormat = ChatResponseFormat.Json
            };

            var response = await chatClient.CompleteAsync(messages, options, cancellationToken);

            var json = response.Message.Text ?? "{}";

            var result = JsonSerializer.Deserialize<AiReviewResult>(json, JsonOptions);

            return result ?? new AiReviewResult([]);
        }

        private static string BuildPrompt(PrDetails prDetails)
        {
            var files = string.Join("\n", prDetails.FilesChanged.Select(f => $"  - {f}"));

            return $$"""
                Review the following pull request and identify issues.

                Title: {{prDetails.Title}}
                Author: {{prDetails.Author}}
                Files Changed:
                {{files}}

                Return ONLY a JSON object in this exact format:
                {
                    "aiFindings": [
                        {
                            "filePath": "path/to/file.cs",
                            "severity": "Critical|Major|Minor|Info",
                            "issue": "Clear description of the issue"
                        }
                    ]
                }

                If no issues are found, return { "aiFindings": [] }.
                """;
        }
    }
}
