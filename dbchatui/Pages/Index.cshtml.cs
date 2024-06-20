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
    public class IndexModel : PageModel
    {
        public string Username { get; set; } 

        public void OnGet()
        {
            // Initialize properties or perform other logic
            Username = "Betsy Teng";
        }
    }

    public class AIQuery
    {
        public string summary { get; set; }
        public string query { get; set; }
    }
}