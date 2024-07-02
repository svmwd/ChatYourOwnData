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

    public class StockPlugin
    {
        private string serviceName = "";
        private string apiKey = "";
        private string indexName = "";

        [KernelFunction]
        [Description("Provide information about food menu and price from this list of hawker stalls: Muslim Delights, Tai Chi Fishball Noodle Stall, Boon Leng Fishball Noodles Stall, Teochew Duck Rice Hawker Stall, Yuan Pin Wanton Mee Hawker Stall, Yan Kee Chicken Rice Hawker Stall")]
        public async Task<List<string>> GetMenuAsync(
        Kernel kernel,
        [Description("User Prompt")] string UserPrompt)
        {
            List<string> menu = new List<string>();

            // Create a SearchIndexClient to send create/delete index commands
            Uri serviceEndpoint = new Uri($"https://{serviceName}.search.windows.net/");
            AzureKeyCredential credential = new AzureKeyCredential(apiKey);
            SearchIndexClient adminClient = new SearchIndexClient(serviceEndpoint, credential);

            // Create a SearchClient to load and query documents
            SearchClient srchclient = new SearchClient(serviceEndpoint, indexName, credential);

            SearchOptions options = new SearchOptions()
            {
                IncludeTotalCount = true,
                Filter = "",
                OrderBy = { "" }
            };

            options.Select.Add("StallName");
            options.Select.Add("ItemName");
            options.Select.Add("UnitPrice");

            SearchResults<MF_STK_PRICE> response = srchclient.Search<MF_STK_PRICE>("*", options);
            //WriteDocuments(response);

            foreach (SearchResult<MF_STK_PRICE> result in response.GetResults().Take(5))
            {
                //Console.WriteLine(result.Document);
                menu.Add(result.Document.ItemName + " @" + result.Document.UnitPrice);
            }

            return menu;
        }

        private static void WriteDocuments(SearchResults<MF_STK_PRICE> searchResults)
        {
            foreach (SearchResult<MF_STK_PRICE> result in searchResults.GetResults())
            {
                Console.WriteLine(result.Document);
            }

            Console.WriteLine();
        }

    }
}
