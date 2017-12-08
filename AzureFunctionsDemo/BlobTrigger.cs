using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace AzureFunctionsDemo
{
    public static class BlobTrigger
    {
        [FunctionName("BlobTrigger")]
        public static async Task Run([BlobTrigger("profile-images/{name}")]CloudBlockBlob myBlob,
            string name, TraceWriter log, [Queue("analysis-results")]IAsyncCollector<AnalysisResult> outputQueueItem)
        {
            var uri = GetUri(myBlob);
            log.Info($"Image URI: {uri}");
            var result = await AnalyseImage(uri);
            log.Info($"Analysis: {result}");
            var resultObj = JsonConvert.DeserializeObject<AnalysisResult>(result);
            await outputQueueItem.AddAsync(resultObj);
        }

        public static Uri GetUri(CloudBlockBlob blob)
        {
            var sasPolicy = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTime.Now.AddMinutes(-30),
                SharedAccessExpiryTime = DateTime.Now.AddMinutes(30)
            };
            var sasToken = blob.GetSharedAccessSignature(sasPolicy);
            var uri = new Uri($"{blob.Uri.ToString()}{sasToken}");
            return uri;
        }

        public static async Task<string> AnalyseImage(Uri imageUri)
        {
            var client = new HttpClient();
            var data = JsonConvert.SerializeObject(new { url = imageUri.ToString() });

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://australiaeast.api.cognitive.microsoft.com/vision/v1.0/analyze?visualFeatures=Description,Color,Adult,ImageType&language=en"),
                Method = HttpMethod.Post
            };

            request.Headers.Add("Ocp-Apim-Subscription-Key", "f8e142eecd7c45fea40ac869a830906e");
            request.Content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}
