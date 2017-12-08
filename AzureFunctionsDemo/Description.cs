using System.Collections.Generic;

namespace AzureFunctionsDemo
{
    public class Description
    {
        public IList<string> Tags { get; set; }
        public IList<Caption> Captions { get; set; }
    }
}