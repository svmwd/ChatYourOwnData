using Azure;
using Azure.AI.OpenAI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Text.Json;
using YourOwnData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using YourOwnData.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static YourOwnData.Models.AzureOAIResult;
using System.Data;
using Azure.Core;

namespace YourOwnData.Pages
{
    public class AzureSPCallModel : PageModel
    {
        [BindProperty]
        public string UserPrompt { get; set; } = string.Empty;
        public string Response { get; set; }
        public string Error { get; set; }

        public void OnPost()
        {
            RunQuery(UserPrompt);
        }

        public void RunQuery(string userPrompt)
        {
            var result = GetPOSTAzureOAIResponse(userPrompt);

            Response = result.Result.Choices[0].Message.Content ?? string.Empty;           
        }

        private static SqlConnection connString()
        {
            //currently azure database is not available
            string strConn = "Server=mwd-openai-server.database.windows.net; User Id=mwdadmin;Password=st@rvisi0n;Database=mwd-openai-db;Connection Timeout=300000;";
            return new SqlConnection(strConn);
        }

        private POSTMessageResponse? GetPOSTAzureOAIResponse(string request)
        {
            POSTMessageResponse? response = new POSTMessageResponse();
            string sp = "UP_Azure_OpenAI";
            using (SqlCommand cmd = new SqlCommand(sp, connString()))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@request", request));

                try
                {
                    cmd.Connection.Open();
                    object obj = cmd.ExecuteScalar();
                    if (obj is not null)
                    {
                        string jsonString = (string)obj;
                        JObject json = JObject.Parse(jsonString);
                        response = JsonConvert.DeserializeObject<POSTMessageResponse>(jsonString);
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
                finally { cmd.Connection.Close(); }
            }

            return response;
        }

    }
}
