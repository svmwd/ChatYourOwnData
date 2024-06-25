using Azure;
using Azure.AI.OpenAI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Text.Json;
using YourOwnData;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using YourOwnData.Models;

namespace YourOwnData.Pages
{
    public class AzureIndexModel : PageModel
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
            string serviceName = "mecwise-azure-openai-search-basic";
            string apiKey = "aG3dbtK1busxxJfF7IMu5oiJIAvCvsUk4bhRY6DfJDAzSeDSb4Wg";
            string indexName = "svstaging-hawker-mf-stk-ai-chat-index";

            // Create a SearchIndexClient to send create/delete index commands
            Uri serviceEndpoint = new Uri($"https://{serviceName}.search.windows.net/");
            AzureKeyCredential credential = new AzureKeyCredential(apiKey);
            SearchIndexClient adminClient = new SearchIndexClient(serviceEndpoint, credential);

            // Create a SearchClient to load and query documents
            SearchClient srchclient = new SearchClient(serviceEndpoint, indexName, credential);

            SearchOptions options;
            SearchResults<MF_STK_PRICE> response;

            // Query 1
            Console.WriteLine("Query #1: Search on empty term '*' to return all documents, showing a subset of fields...\n");

            options = new SearchOptions()
            {
                IncludeTotalCount = true,
                Filter = "",
                OrderBy = { "" }
            };

            options.Select.Add("StallName");
            options.Select.Add("ItemName");
            options.Select.Add("UnitPrice");

            response = srchclient.Search<MF_STK_PRICE>("*", options);
            var check = response.GetResults().AsQueryable();

            var rows = new List<List<string>>();
            var headerCols = new List<string>();
            headerCols = options.Select.ToList();
            rows.Add(headerCols);

            int nCol = options.Select.Count;

            var cols = new List<string>();      
            foreach ( var rec in check)
            {
                cols.Add(rec.Document.StallName);
                cols.Add(rec.Document.ItemName);
                cols.Add(rec.Document.UnitPrice);
            }
            
            rows.Add(cols);

            RowData = rows;
        }
    }

   
}