using System;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ServiceBusQueueFunction
{
    public static class MyTrigger
    {
        [FunctionName("MyTrigger")]
        // myQueueItem can be string
        //public static async Task RunAsync([ServiceBusTrigger("mainqueue", Connection = "sb-connection")]
        //    string myQueueItem, ILogger log)

        //  myQueue Itemcan be Message
        public static async Task RunAsync([ServiceBusTrigger("mainqueue", Connection = "sb-connection")]
            Message myQueueItem, ILogger log)

        
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            
        }
    }
}