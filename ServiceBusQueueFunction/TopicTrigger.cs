using System;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ServiceBusQueueFunction
{
    public static class TopicTrigger
    {
        [FunctionName("TopicTrigger")]
        public static async Task RunAsync([ServiceBusTrigger("mytopic", "Sub1", Connection = "topic-connection")]
            string mySbMsg, ILogger log)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            
        }
    } 
}