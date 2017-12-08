using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctionsDemo
{
    public static class VertifyImageDetails
    {
        [FunctionName("VertifyImageDetails")]
        public static void Run([QueueTrigger("analysis-results")]
            AnalysisResult analysisResult,
            TraceWriter log, [Table("BadImagesTable")]ICollector<AnalysisResult> outputTable)
        {
            if (analysisResult.Adult.IsRacyContent)
            {
                analysisResult.PartitionKey = "AnalysisResults";
                analysisResult.RowKey = analysisResult.RequestId.ToString();
                outputTable.Add(analysisResult);
            }
        }
    }
}
