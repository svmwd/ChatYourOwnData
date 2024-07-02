using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;
using System.ComponentModel;
using Azure.AI.OpenAI;

namespace YourOwnData.Plugins
{

    public class UserGuidePlugin
    {
        private string DataSourceEndpoint = "";
        private string DataSourceApiKey = "";
        private string DataSourceIndex = "";

        [KernelFunction]
        [Description("Provide information about mecwise application to a recipient based on userguides uploaded in this datasource")]
        public async Task<string> GetStringAsync(
        Kernel kernel,
        [Description("User Prompt")] string UserPrompt)
        {
#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            var azureSearchExtensionConfiguration = new AzureSearchChatExtensionConfiguration
            {
                SearchEndpoint = new Uri(DataSourceEndpoint),
                Authentication = new OnYourDataApiKeyAuthenticationOptions(DataSourceApiKey),
                IndexName = Environment.GetEnvironmentVariable(DataSourceIndex)
            };

            var chatExtensionsOptions = new AzureChatExtensionsOptions { Extensions = { azureSearchExtensionConfiguration } };
            var executionSettings = new OpenAIPromptExecutionSettings { AzureChatExtensionsOptions = chatExtensionsOptions };

            var result = await kernel.InvokePromptAsync(UserPrompt, new(executionSettings));

#pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            return result.ToString();
        }

    }
}
