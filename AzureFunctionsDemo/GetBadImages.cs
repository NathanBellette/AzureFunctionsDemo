using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctionsDemo
{
    public static class GetBadImages
    {
        [FunctionName("GetBadImages")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
            [Table("BadImagesTable", "AnalysisResults")] IQueryable<AnalysisResult> analysisResults,
            TraceWriter log)
        {
            var results = await analysisResults.ToList
            return req.CreateResponse(HttpStatusCode.OK, analysisResults.ToList());
        }
    }
}
