using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourOwnData.Models
{
    public class AzureOAIResult
    {
        public class POSTMessageResult
        {
            [JsonProperty("ReturnCode")]
            public int ReturnCode { get; set; }

            [JsonProperty("Response")]
            public string Response { get; set; }

            /*
            [JsonProperty("salesdetails")]
            public IEnumerable<POSTJSalesDetailResult> JSalesDetails { get; set; }

            [JsonProperty("payments")]
            public IEnumerable<POSTJPaymentResult> JPayments { get; set; }

            public POSTJSalesMasterResult()
            {
                SMID = 0;
                LocalSMID = 0;
                JSalesDetails = new List<POSTJSalesDetailResult>();
                JPayments = new List<POSTJPaymentResult>();
            }
            */
        }

        public class POSTMessageResponse
        {
            [JsonProperty("response")]
            public CResponse Response { get; set; }
            [JsonProperty("result")]
            public CResult Result { get; set; }
            /*
            [JsonProperty("salesdetails")]
            public IEnumerable<POSTJSalesDetailResult> JSalesDetails { get; set; }

            [JsonProperty("payments")]
            public IEnumerable<POSTJPaymentResult> JPayments { get; set; }

            public POSTJSalesMasterResult()
            {
                SMID = 0;
                LocalSMID = 0;
                JSalesDetails = new List<POSTJSalesDetailResult>();
                JPayments = new List<POSTJPaymentResult>();
            }
            */
        }

        public class CResponse
        {
            [JsonProperty("status")]
            public CStatus Status { get; set; }

            [JsonProperty("headers")]
            public CHeaders Headers { get; set; }
        }

        public class CStatus
        {
            [JsonProperty("http")]
            public CHttp Http { get; set; }
        }

        public class CHttp
        {
            [JsonProperty("code")]
            public int Code { get; set; }
            [JsonProperty("description")]
            public string Description { get; set; }
        }

        public class CHeaders
        {
            [JsonProperty("Cache-Control")]
            public string CacheControl { get; set; }
            [JsonProperty("Date")]
            public string Date { get; set; }
            [JsonProperty("Content-Length")]
            public int ContentLength { get; set; }
            [JsonProperty("Content-Type")]
            public string ContentType { get; set; }
            [JsonProperty("access-control-allow-origin")]
            public string AccessControlAllowOrigin { get; set; }
            [JsonProperty("apim-request-id")]
            public string ApimRequestId { get; set; }
            [JsonProperty("strict-transport-security")]
            public string StrictTransportSecurity { get; set; }
            [JsonProperty("x-content-type-options")]
            public string XContentTypeOptions { get; set; }
            [JsonProperty("x-ms-region")]
            public string XMsRegion { get; set; }
            [JsonProperty("x-ratelimit-remaining-requests")]
            public int XRatelimitRemainingRequests { get; set; }
            [JsonProperty("x-ratelimit-remaining-tokens")]
            public int XRatelimitRemainingTokens { get; set; }
            [JsonProperty("x-accel-buffering")]
            public string XAccelBuffering { get; set; }
            [JsonProperty("x-ms-rai-invoked")]
            public bool XMsRaiInvoked { get; set; }
            [JsonProperty("x-request-id")]
            public string XRequestId { get; set; }
            [JsonProperty("x-ms-client-request-id")]
            public string XMsClientRequestId { get; set; }
            [JsonProperty("azureml-model-session")]
            public string AzureMlModelSession { get; set; }
        }

        public class CResult
        {
            public List<CChoice> Choices { get; set; }
            public long Created { get; set; }
            public string Id { get; set; }
            public string Model { get; set; }
            public string Object { get; set; }
            public List<CPromptFilterResult> PromptFilterResults { get; set; }
            public string SystemFingerprint { get; set; }
            public Usage Usage { get; set; }
        }

        public class CChoice
        {
            public CContentFilterResults ContentFilterResults { get; set; }
            public string FinishReason { get; set; }
            public int Index { get; set; }
            public object Logprobs { get; set; }
            public CMessage Message { get; set; }
        }

        public class CContentFilterResults
        {
            public CContentFilterResult Hate { get; set; }
            public CContentFilterResult SelfHarm { get; set; }
            public CContentFilterResult Sexual { get; set; }
            public CContentFilterResult Violence { get; set; }
        }

        public class CContentFilterResult
        {
            public bool Filtered { get; set; }
            public string Severity { get; set; }
        }

        public class CMessage
        {
            public string Content { get; set; }
            public string Role { get; set; }
        }

        public class CPromptFilterResult
        {
            public int PromptIndex { get; set; }
            public CContentFilterResults ContentFilterResults { get; set; }
        }

        public class Usage
        {
            public int CompletionTokens { get; set; }
            public int PromptTokens { get; set; }
            public int TotalTokens { get; set; }
        }

    }
}
