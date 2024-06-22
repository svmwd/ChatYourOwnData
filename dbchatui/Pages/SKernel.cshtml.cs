using Azure;
using Azure.AI.OpenAI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using YourOwnData.Plugins;

namespace YourOwnData.Pages
{
    public class SKernelModel : PageModel
    {
        [BindProperty]
        public string UserPrompt { get; set; } = string.Empty;
        public string? Response { get; set; }
        public string? Error { get; set; }

        public void OnGet() 
        {
        }

        public void OnPost()
        {
            RunQuery(UserPrompt);
        }

        public void OnPostAttachFileAsync()
        {
            var uploadedFile = Request.Form.Files["fileInput"];
            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                // Process the uploaded file (e.g., save it, read its content, etc.)
                // Example: Save the file to disk
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", uploadedFile.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadedFile.CopyTo(stream);
                }
            }
        }

        public void RunQuery(string userPrompt)
        {
            GlobalValues.history.AddUserMessage(userPrompt);
            
            // Get the response from the AI
            var result = GlobalValues.chatCompletionService.GetChatMessageContentAsync(
                GlobalValues.history,
                executionSettings: GlobalValues.openAIPromptExecutionSettings,
                kernel: GlobalValues.kernel);
                 
            Response = result.Result.Content ?? string.Empty;
            GlobalValues.history.AddAssistantMessage(Response);
        }


    }

}
