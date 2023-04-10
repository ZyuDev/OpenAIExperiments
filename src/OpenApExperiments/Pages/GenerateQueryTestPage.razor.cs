using AntDesign;
using Microsoft.AspNetCore.Components;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenApExperiments.Infrastructure;

namespace OpenApExperiments.Pages
{
    public partial class GenerateQueryTestPage : ComponentBase
    {
        private string _promptText;
        private string _queryText;

        [Inject]
        public NotificationService Notify { get; set; }

        [Inject]
        public IAppSettings AppSettings { get; set; }

        private async Task OnGoClick()
        {
            await Notify.Open(new NotificationConfig()
            {
                Message = "Go",
                Description = "Go pressed",
                NotificationType = NotificationType.Info,
            });


            var openAiService = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = AppSettings.ApiToken,

            });

            var completionResult = await openAiService.Completions.CreateCompletion(new CompletionCreateRequest()
            {
                Prompt = _promptText,
                Model = Models.ChatGpt3_5Turbo
            });

            if (completionResult.Successful)
            {
                Console.WriteLine(completionResult.Choices.FirstOrDefault());

                _queryText = completionResult.Choices.FirstOrDefault()?.Text;

            }
            else
            {
                if (completionResult.Error == null)
                {
                    throw new Exception("Unknown Error");
                }
                var message = $"{completionResult.Error.Code}: {completionResult.Error.Message}";

                _queryText = message;
                Console.WriteLine(message);
            }

        }

        private async Task OnChatClick()
        {
            var openAiService = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = AppSettings.ApiToken,

            });

            var completionResult = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
    {
        ChatMessage.FromUser(_promptText),
    },
                Model = Models.ChatGpt3_5Turbo
            });
            if (completionResult.Successful)
            {
                Console.WriteLine(completionResult.Choices.First().Message.Content);
                _queryText = completionResult.Choices.First().Message.Content;
            }
            else
            {
                if (completionResult.Error == null)
                {
                    throw new Exception("Unknown Error");
                }

                var message = $"{completionResult.Error.Code}: {completionResult.Error.Message}";
                Console.WriteLine(message);

                _queryText= message;    
            }

        }

    }
}
