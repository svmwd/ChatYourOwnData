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

            string openAIEndpoint = "https://mwd-azure-openai-westus.openai.azure.com/";
            string openAIKey = "3e0982201b4f44819ab2003f3d179d32";

            // Configure OpenAI client GPT-4o
            string openAIDeploymentName = "mwd-azure-openai-gpt-4o";

            // Configure OpenAI client GPT-35
            //string openAIDeploymentName = "mwd-kernel-plugin";

            OpenAIClient client = new(new Uri(openAIEndpoint), new AzureKeyCredential(openAIKey));
           
            // Set up the AI chat query/completion
            ChatCompletionsOptions chatCompletionsOptions = new ChatCompletionsOptions()
            {
                Messages = {
                    new ChatRequestSystemMessage(systemMessage),
                    new ChatRequestUserMessage(userPrompt)
                },
                DeploymentName = openAIDeploymentName
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
