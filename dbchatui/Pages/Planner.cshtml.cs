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
using Microsoft.SemanticKernel.Planning.Handlebars;
using Microsoft.SemanticKernel.Planning;
using System;
using System.Numerics;

namespace YourOwnData.Pages
{
    public class PlannerModel : PageModel
    {
        [BindProperty]
        public string UserPrompt { get; set; } = string.Empty;
        public string Response { get; set; }
        public string? Error { get; set; }

        public void OnGet() 
        {
        }

        public async Task OnPost()
        {
            await RunQuery(UserPrompt);
        }


        public async Task RunQuery(string userPrompt)
        {
            Kernel kernel = GlobalValues.kernel;

            GlobalValues.UserMessage = userPrompt;
            GlobalValues.history.AddUserMessage(userPrompt);

            OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

            
#pragma warning disable SKEXP0060 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            var plannerOptions = new HandlebarsPlannerOptions()
            {
                // When using OpenAI models, we recommend using low values for temperature and top_p to minimize planner hallucinations.
                ExecutionSettings = new OpenAIPromptExecutionSettings()
                {
                    Temperature = 0.0,
                    TopP = 0.1,
                },
                // Use gpt-4 or newer models if you want to test with loops.
                // Older models like gpt-35-turbo are less recommended. They do handle loops but are more prone to syntax errors.
                AllowLoops = true,
            };

            try
            {
                //string goal = "to provide food suggestion and email order";
                var planner = new HandlebarsPlanner(plannerOptions);
                var plan = await planner.CreatePlanAsync(kernel, userPrompt);

                Response = await plan.InvokeAsync(kernel);

                //Response = "Step by Step Plan: " + plan;
                //Response += "Result: " + planResult ?? string.Empty;
                GlobalValues.history.AddAssistantMessage(Response);
            }
            catch (Microsoft.SemanticKernel.Planning.PlanCreationException e)
            {
                var errorDetails = e.InnerException?.Message;
                var promptDetails = e.CreatePlanPrompt;
                var modelResults = e?.ModelResults?.Content; // Proposed plan
            }

#pragma warning restore SKEXP0060 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        }


    }

}
