using Azure.AI.OpenAI;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using YourOwnData.Services;

namespace YourOwnData.Pages
{
    public class SQLAuditLogModel : PageModel
    {
            [BindProperty]
            public string UserPrompt { get; set; } = string.Empty;
            public List<List<string>> RowData { get; set; }
            public string Summary { get; set; }
            public string Query { get; set; }
            public string Error { get; set; }

            public void OnPost()
            {
                RunQuery(UserPrompt);
            }

            public void RunQuery(string userPrompt)
            {
                string openAIEndpoint = "https://mwd-azure-openai-westus.openai.azure.com/";
                string openAIKey = "3e0982201b4f44819ab2003f3d179d32";

                // Configure OpenAI client GPT-4
                string openAIDeploymentName = "mwd-azure-openai-gpt-4o";

                // Configure OpenAI client GPT-35
                //string openAIDeploymentName = "mwd-kernel-plugin";

                OpenAIClient client = new(new Uri(openAIEndpoint), new AzureKeyCredential(openAIKey));

                // Use the SchemaLoader project to export your db schema and then paste the schema in the placeholder below
                var systemMessage = @"Your are a helpful, cheerful database assistant. 
            Use the following database schema when creating your answers:
            - dbo.SQL_AUDIT_LOG ([database_name],transaction_id, [object_id], event_time,  [object_name], [statement] )
       
            Include column name headers in the query results.

            Always provide your answer in the JSON format below:
            
            { ""summary"": ""your-summary"", ""query"":  ""your-query"" }
            
            Output ONLY JSON.
            In the preceding JSON response, substitute ""your-query"" with Microsoft SQL Server Query to retrieve the requested data.
            In the preceding JSON response, substitute ""your-summary"" with a summary of the query.            
            If the resulting query is non-executable, replace ""your-query"" with NA, but still substitute ""your-query"" with a summary of the query.
            Do not use MySQL syntax.";

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

                    Summary = response.summary;
                    Query = response.query;
                    RowData = DataService.GetDataTable(response.query);
                }
                catch (Exception e)
                {
                    Error = e.Message;
                }
            }
        }


    }

