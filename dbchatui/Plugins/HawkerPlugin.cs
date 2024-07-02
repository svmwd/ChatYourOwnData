using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;
using System.ComponentModel;
using Azure.AI.OpenAI;
using Azure.Search.Documents.Models;
using Azure.Search.Documents;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using YourOwnData.Models;
using Azure.Search.Documents.Indexes;
using Azure;

namespace YourOwnData.Plugins
{

    public class HawkerPlugin
    {
        private string serviceName = "";
        private string apiKey = "";
        private string indexName = "";

        [KernelFunction]
        [Description("Provide information about food menu and price from this list of hawker stalls: " +
            "1. Muslim Delights " +
            "2. Tai Chi Fishball Noodle Stall " +
            "3. Boon Leng Fishball Noodles Stall " +
            "4. Teochew Duck Rice Hawker Stall " +
            "5. Yuan Pin Wanton Mee Hawker Stall " +
            "6. Yan Kee Chicken Rice Hawker Stall. " +
            "Format your response to user neatly, in point form or table form if more than one choices are available." +
            "Suggest food to eat if needed")]
        public async Task<FunctionResult> GetMenuAsync(
        Kernel kernel,
        [Description("This is user prompt or input")] string userInput)
        {
            var azureSearchExtensionConfiguration = new AzureSearchChatExtensionConfiguration
            {
                SearchEndpoint = new Uri($"https://{serviceName}.search.windows.net/"),
                Authentication = new OnYourDataApiKeyAuthenticationOptions(apiKey),
                IndexName = indexName
            };

#pragma warning disable SKEXP0010

            var chatExtensionsOptions = new AzureChatExtensionsOptions { Extensions = { azureSearchExtensionConfiguration } };
            var executionSettings = new OpenAIPromptExecutionSettings { AzureChatExtensionsOptions = chatExtensionsOptions };

#pragma warning restore SKEXP0010
          
            var result = await kernel.InvokePromptAsync(userInput, new(executionSettings));
            return result;
        }

    }
}
