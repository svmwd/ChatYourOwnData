using Azure;
using Azure.AI.OpenAI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using YourOwnData.Plugins;

namespace YourOwnData.Pages
{
    public class SKernelModel : PageModel
    {
        [BindProperty]
        public string UserPrompt { get; set; } = string.Empty;
        public string Response { get; set; }
        public string Error { get; set; }


        private string modelId = "mwd-azure-openai-gpt-4o";
        private string endpoint = "https://mwd-azure-openai-westus.openai.azure.com/";
        private string apiKey = "3e0982201b4f44819ab2003f3d179d32";
        private Kernel kernel;

        public void OnGet() { }

        public void OnPost()
        {
            RunQuery(UserPrompt);
        }

        public void RunQuery(string userPrompt)
        {
            // Create chat history
            var history = new ChatHistory();

            // Get chat completion service
            var builder = Kernel.CreateBuilder()
       .AddAzureOpenAIChatCompletion(modelId, endpoint, apiKey);
            builder.Plugins.AddFromType<MenuPlugin>();
            kernel = builder.Build();

            var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

            // Use the SchemaLoader project to export your db schema and then paste the schema in the placeholder below
            var systemMessage = @"Your are a helpful, cheerful and friendly assistant";

            history.AddUserMessage(userPrompt);

            // Enable auto function calling
            OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
                 ChatSystemPrompt = systemMessage
            };

            // Get the response from the AI
            var result = chatCompletionService.GetChatMessageContentAsync(
                history,
                executionSettings: openAIPromptExecutionSettings,
                kernel: kernel);
                 
            Response = result.Result.Content ?? string.Empty;
        }


        public async void OnConsole()
        {
            // Create chat history
            var history = new ChatHistory();

            // Get chat completion service
            var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

            // Start the conversation
            Console.Write("User > ");
            string? userInput;
            while ((userInput = Console.ReadLine()) != null)
            {
                // Add user input
                history.AddUserMessage(userInput);

                // Enable auto function calling
                OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
                {
                    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
                };

                // Get the response from the AI
                var result = await chatCompletionService.GetChatMessageContentAsync(
                    history,
                    executionSettings: openAIPromptExecutionSettings,
                    kernel: kernel);

                // Print the results
                Console.WriteLine("Assistant > " + result);

                // Add the message from the agent to the chat history
                history.AddMessage(result.Role, result.Content ?? string.Empty);

                // Get user input again
                Console.Write("User > ");
            }
        }

    }

}
