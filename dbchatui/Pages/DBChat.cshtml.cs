using Azure;
using Azure.AI.OpenAI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using YourOwnData.Services;

namespace YourOwnData.Pages
{
    public class DBChatModel : PageModel
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

            // Configure OpenAI client GPT-4o
            string openAIDeploymentName = "mwd-azure-openai-gpt-4o";

            // Configure OpenAI client GPT-35
            //string openAIDeploymentName = "mwd-kernel-plugin";

            OpenAIClient client = new(new Uri(openAIEndpoint), new AzureKeyCredential(openAIKey));

            // Use the SchemaLoader project to export your db schema and then paste the schema in the placeholder below
            var systemMessage = @"Your are a helpful, cheerful database assistant. 
            Use the following database schema when creating your answers:

            If the question is about food or item, please use this database schema:
            - dbo.MF_COMP (COMP_CODE, COMP_NAME )
            - dbo.MF_STK (COMP_CODE, PART_DESC, PART_GRP, PART_LONG_DESC, PART_NO, REVI_PRCE )
            - dbo.MF_STK_GRP (COMP_CODE, PART_GRP, PART_GRP_DESC )

            else if the question is about user menu, then use this database schema:
            - dbo.SV_MF_SMAN (COMP_CODE, SMAN_CODE, SMAN_NAME, LOGIN_ID)
            - dbo.SV_SC_USER (USER_ID, GRP_ID)
            - dbo.SV_SC_MDULE_MATRIX (GRP_ID, GRP_DESC, MENU_SUB_ITEM_NAME_0, MENU_SUB_ITEM_DTL_NAME_0)

            else if the question is about sales, then use this database schema:
            - dbo.SV_AR_INV_OPENAI_TESTING ( [COMPANY NAME],  [CUSTOMER NAME], [PART GROUP NAME], [PART NAME], QTY, [UNIT PRICE], [TOTAL AMOUNT], [CREATE DATE])

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
