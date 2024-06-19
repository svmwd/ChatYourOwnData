using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace YourOwnData.Plugins
{
    public class UserGuidePlugin
    {
        [KernelFunction]
        [Description("Provide information to a recipient based on datasource")]
        public async Task<string> GetPrice(string itemName)
        {
            if (itemName == null)
            {
                return "0";
            }
            else
            {
#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

                Kernel kernel = Kernel.CreateBuilder()
                        .AddAzureOpenAIChatCompletion(new AzureOpenAIChatCompletionWithDataConfig
                        {
                            CompletionModelId = "gpt-35-turbo-1106",
                            CompletionEndpoint = "https://mwd-azure-openai-westus.openai.azure.com/",
                            CompletionApiKey = "3e0982201b4f44819ab2003f3d179d32",
                            DataSourceEndpoint = "https://mecwise-azure-openai-chatbot-search-basic.search.windows.net",
                            DataSourceApiKey = "jgvyZQ2Uvef0lZLisLU5Upe3yMWmunH3wU7aTrLGLPAzSeBwxI1i",
                            DataSourceIndex = "mecwise-azure-openai-chatbot-search-basic-index01"
                        })
                        .Build();
#pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

                var result = await kernel.InvokePromptAsync("how much is " + itemName);

                return result.ToString();
            }
        }
    }
}
