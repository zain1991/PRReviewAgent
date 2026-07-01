using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;
using PrReviewAgent.Application.Common.Interfaces;
using PrReviewAgent.Application.Common.Models;

namespace PrReviewAgent.Infrastructure.Services
{
    public class OllamaReviewService(ChatClient chatClient, ILogger<OllamaReviewService> logger) : IAiReviewService
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<AiReviewResult> ReviewPullRequestAsync(PrDetails prDetails, CancellationToken cancellationToken)
        {
            var messages = new List<ChatMessage>
            {
                new SystemChatMessage("""
                    You are a senior code reviewer. Analyze the pull request details provided and return findings as JSON only.
                    Do not include any explanation, markdown, or text outside the JSON object.
                    Severity must be one of: Critical, Major, Minor, Info.
                    """),
                new UserChatMessage(BuildPrompt(prDetails))
            };

            var options = new ChatCompletionOptions
            {
                ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat()
            };

            var response = await chatClient.CompleteChatAsync(messages, options, cancellationToken);

            var rawText = response.Value.Content[0].Text ?? "{}";
            logger.LogDebug("Raw AI response: {Response}", rawText);

            var json = StripThinkingTags(rawText);

            var result = JsonSerializer.Deserialize<AiReviewResult>(json, JsonOptions);

            return result ?? new AiReviewResult([]);
        }

        // qwen3 wraps responses in <think>...</think> before the JSON when thinking mode is active
        private static string StripThinkingTags(string text)
        {
            var stripped = Regex.Replace(text, @"<think>[\s\S]*?</think>", string.Empty).Trim();
            return string.IsNullOrWhiteSpace(stripped) ? "{}" : stripped;
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
