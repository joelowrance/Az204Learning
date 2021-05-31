using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ServiceBusQueueFunction
{
    public static class MyTrigger
    {
        [FunctionName("MyTrigger")]
        public static async Task RunAsync([ServiceBusTrigger("mainqueue", Connection = "sb-connection")]
            string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            
        }
    }
}