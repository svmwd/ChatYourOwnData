using Azure.AI.OpenAI;
using Azure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using YourOwnData.Models;
using YourOwnData.Pages;
using System.Text.Json;
using System.Reflection.Metadata.Ecma335;

namespace YourOwnData.Services
{
    public static class QueryService
    {
        public static string GetQuery(string userPrompt, string systemMessage)
        {
            string sQuery = string.Empty;

            OpenAIClient client = GlobalValues.openAIClient;

            // Set up the AI chat query/completion
            ChatCompletionsOptions chatCompletionsOptions = new ChatCompletionsOptions()
            {
                Messages = {
                    new ChatRequestSystemMessage(systemMessage),
                    new ChatRequestUserMessage(userPrompt)
                },
                DeploymentName = GlobalValues.AzureAIDeployment
            };


            // Send request to Azure OpenAI model
            try
            {
                ChatCompletions chatCompletionsResponse = client.GetChatCompletions(chatCompletionsOptions);

                var response = JsonSerializer
                    .Deserialize<AIQuery>(chatCompletionsResponse.Choices[0].Message.Content
                    .Replace("```json", "").Replace("```", ""));

                //Summary = response.summary;
                sQuery = response.query;
                //list = DataService.GetDataTable(response.query);
            }
            catch (Exception e)
            {
                //Error = e.Message;
            }
            return sQuery;
        }
        
    }
}
