using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using YourOwnData.Plugins;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Azure.Search.Documents;
using Azure;
using Elastic.Transport;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Azure.AI.OpenAI;

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

        private static Uri? _azureSearchEndPoint;
        private static AzureKeyCredential? _azureSearchCredential;
        private static string? _azureSearchUserGuides;
        private static string? _azureSearchHawkerMenu;

        private static string? _connString;

        private static Kernel? _kernel;
        private static ChatHistory? _history;
        private static string? _systemMessage;
        private static string? _userMessage;
        private static IChatCompletionService? _chatCompletionService;
        private static OpenAIPromptExecutionSettings? _openAIPromptExecutionSettings;
        private static SearchClient? _searchClient;
        private static OpenAIClient? _openAIClient;

        

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
        public static string AzureAIDeployment
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

        private static Uri AzureSearchEndPoint
        {
            get
            {
                if (_azureSearchEndPoint == null)
                {
                    string str = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AZURESEARCH")["ENDPOINT"];
                    _azureSearchEndPoint = new Uri($"https://{str}.search.windows.net/");
                }
                return _azureSearchEndPoint;
            }
        }

        private static AzureKeyCredential AzureSearchCredential
        {
            get
            {
                if (_azureSearchCredential == null)
                {
                    string apiKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AZURESEARCH")["APIKEY"];
                    _azureSearchCredential = new AzureKeyCredential(apiKey);
                }
                return _azureSearchCredential;
            }
        }

        private static string AzureSearchUserGuides
        {
            get
            {
                if (_azureSearchUserGuides == null)
                {
                    _azureSearchUserGuides = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AZURESEARCH")["USERGUIDES"];
                }
                return _azureSearchUserGuides;
            }
        }

        private static string AzureSearchHawkerMenu
        {
            get
            {
                if (_azureSearchHawkerMenu == null)
                {
                    _azureSearchHawkerMenu = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AZURESEARCH")["HAWKERMENU"];
                }
                return _azureSearchHawkerMenu;
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
                        builder.Plugins.AddFromType<HawkerPlugin>();
                        //builder.Plugins.AddFromType<UserGuidePlugin>();
                        builder.Plugins.AddFromType<PrintPlugin>();
                        builder.Plugins.AddFromType<EmailPlugin>();
                        builder.Plugins.AddFromType<WebLaunchPlugin>();
                        _kernel = builder.Build();
                }
                return _kernel;
            }
        }
        public static SearchClient searchClient(string datasource)
        {
            string s = datasource.Trim().ToLower();
            switch (s)
            {
                case "userguides":
                    return new SearchClient(AzureSearchEndPoint, AzureSearchUserGuides, AzureSearchCredential);

                case "hawkermenu":
                    return new SearchClient(AzureSearchEndPoint, AzureSearchHawkerMenu, AzureSearchCredential);

                 default:
                    return new SearchClient(AzureSearchEndPoint, AzureSearchUserGuides, AzureSearchCredential);
            }
        }

        public static OpenAIClient openAIClient
        {
            get
            {
                if (_openAIClient == null)
                {
                    _openAIClient = new(new Uri(AzureAIEndPoint), new AzureKeyCredential(AzureAIAPIKey));
                }
                return _openAIClient;
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

        public static string UserMessage
        {
            get
            {
                if (_userMessage == null)
                {
                    _userMessage = string.Empty;
                }
                return _userMessage;
            }
            set { _userMessage = value; }
        }

        public enum EDatabase
        {
            MWDOpenAIDB,
            SVStagingITE,
            HAWKERDB
        }

        public static string ConnectionString(EDatabase db)
        {
            string s = string.Empty;

            switch (db)
            {
                case EDatabase.MWDOpenAIDB:
                    s = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("CONNECTIONSTRING")["mwd-openai-db"];
                    break;

                case EDatabase.SVStagingITE:
                    s = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("CONNECTIONSTRING")["svstaging-ite"];
                    break;

                case EDatabase.HAWKERDB:
                    s = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("CONNECTIONSTRING")["mwp-105-hawker"];
                    break;

                default:
                    break;
            }

            return s;
        }


        public static SMTP GetSMTP()
        {
            SMTP smtp = new SMTP();
            smtp.HostName = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("SMTP")["HOSTNAME"];
            smtp.UserName = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("SMTP")["USERNAME"];
            smtp.Password = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("SMTP")["PASSWORD"];
            smtp.EmailFrom = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("SMTP")["EMAILFROM"];
            return smtp;
        }

    }

}

public class SMTP
{
    public string HostName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string EmailFrom { get; set; }

}
