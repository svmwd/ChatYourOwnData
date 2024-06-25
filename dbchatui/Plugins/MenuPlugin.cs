using Azure;
using Azure.AI.OpenAI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using YourOwnData.Models;
using YourOwnData.Pages;
using YourOwnData.Services;
using System;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace YourOwnData.Plugins
{
    public sealed class MenuPlugin
    {
        private const string SYSTEM_MESSAGE = @"Your are a helpful, cheerful database assistant. 
            Use the following database schema when creating your answers:
            - dbo.MF_COMP (COMP_CODE, COMP_NAME )
            - dbo.MF_STK (COMP_CODE, PART_DESC, PART_GRP, PART_LONG_DESC, PART_NO, REVI_PRCE )
            - dbo.MF_STK_GRP (COMP_CODE, PART_GRP, PART_GRP_DESC )

            SELECT only a maximum of 10 records;
            DO NOT retrieve more than 10 records at a time;
 Include column name headers in the query results.

            Always provide your answer in the JSON format below:
            
            { """"summary"""": """"your-summary"""", """"query"""":  """"your-query"""" }
            
            Output ONLY JSON.
            In the preceding JSON response, substitute """"your-query"""" with Microsoft SQL Server Query to retrieve the requested data.
            In the preceding JSON response, substitute """"your-summary"""" with a summary of the query.
            If the resulting query is non-executable, replace """"your-query"""" with NA, but still substitute """"your-query"""" with a summary of the query.
            Do not use MySQL syntax.""";
           

        [KernelFunction]
        [Description("Based on SQL Query, provides a list of food menu from a number of stalls in a hawker center or foodcourt")]
        public string GetListOfFood(
                                        Kernel kernel,
                                        [Description("User is supposed to provide you with either itemName or stallName")] 
                                        string itemName, 
                                        string stallName)
        {
            string sQuery = QueryService.GetQuery(GlobalValues.UserMessage, SYSTEM_MESSAGE);
            List<List<string>> list = DataService.GetDataTable(sQuery);
            string jsonData = JsonSerializer.Serialize(list);

            GlobalValues.history.AddSystemMessage(jsonData);

            // Get the response from the AI
            var result = GlobalValues.chatCompletionService.GetChatMessageContentAsync(
                GlobalValues.history,
                executionSettings: GlobalValues.openAIPromptExecutionSettings,
                kernel: GlobalValues.kernel);

            string response = result.Result.Content ?? string.Empty;
            GlobalValues.history.AddAssistantMessage(response);

            return response;
        }

        [KernelFunction, Description("Provides the price of the requested menu item.")]
        public string GetItemPrice(
            [Description("The name of the menu item.")]
        string menuItem)
        {
            return "$9.99";
        }
    }
}
