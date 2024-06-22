using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using YourOwnData.Plugins;
using Microsoft.IdentityModel.Tokens;

namespace YourOwnData
{
    public class GlobalValues
    {
        /*datasource*/
        private const string searchName = "mwd-azure-ai-search";
        private const string primaryKey = "H45RR4SK9U64ELTjtjYOZXmtos46jDRAPmwxJeMyRHAzSeCCUsPK";
        private const string indexName = "azuresql-index01";

        private static string? _azureAIEndPoint;
        private static string? _azureAIAPIKey;
        private static string? _azureAIDeployment;

        private static Kernel? _kernel;
        private static ChatHistory? _history;
        private static string? _systemMessage;
        private static IChatCompletionService? _chatCompletionService;
        private static OpenAIPromptExecutionSettings? _openAIPromptExecutionSettings;

        private static string AzureAIEndPoint
        {
            get
            {
                if (_azureAIEndPoint == null)
                {
                    _azureAIEndPoint = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AZUREAI")["ENDPOINT"];
                }
                return _azureAIEndPoint;
            }
        }

        private static string AzureAIAPIKey
        {
            get
            {
                if (_azureAIAPIKey == null)
                {
                    _azureAIAPIKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AZUREAI")["APIKEY"];
                }
                return _azureAIAPIKey;
            }
        }
        private static string AzureAIDeployment
        {
            get
            {
                if (_azureAIDeployment == null)
                {
                    _azureAIDeployment = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AZUREAI")["DEPLOYMENT"];
                }
                return _azureAIDeployment;
            }
        }

        public static string systemMessage
        { 
            get
            {
                if (_systemMessage == null)
                {
                    _systemMessage = @"Your are a helpful, cheerful and friendly assistant"; 
                }
                return _systemMessage;
            }
        }

        public static Kernel kernel
        {
            get
            {
                if (_kernel == null)
                {
                        var builder = Kernel.CreateBuilder()
                        .AddAzureOpenAIChatCompletion(AzureAIDeployment, AzureAIEndPoint, AzureAIAPIKey);
                        builder.Plugins.AddFromType<MenuPlugin>();
                        builder.Plugins.AddFromType<UserGuidePlugin>();
                        builder.Plugins.AddFromType<PrintPlugin>();
                        builder.Plugins.AddFromType<EmailPlugin>();
                        builder.Plugins.AddFromType<WebLaunchPlugin>();
                        _kernel = builder.Build();
                }
                return _kernel;
            }
        }

        public static ChatHistory history
        {
            get
            {
                if (_history == null)
                {
                    _history = new ChatHistory();
                }
                return _history;
            }
        }

        public static IChatCompletionService chatCompletionService 
        { 
            get
            {
                if (_chatCompletionService == null)
                {
                    _chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
                }
                return _chatCompletionService;
            } 
        }

        public static OpenAIPromptExecutionSettings openAIPromptExecutionSettings 
        { 
            get
            {
                if (_openAIPromptExecutionSettings == null)
                {
                    _openAIPromptExecutionSettings = new()
                    {
                        ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
                        ChatSystemPrompt = systemMessage
                    };
                }
                return _openAIPromptExecutionSettings;
            }
        }



    }

}
