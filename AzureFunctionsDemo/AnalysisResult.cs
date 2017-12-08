using System;

namespace AzureFunctionsDemo
{
    public class AnalysisResult
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public Description Description { get; set; }
        public Adult Adult { get; set; }
        public Guid RequestId { get; set; }
        public Metadata Metadata { get; set; }
    }
}