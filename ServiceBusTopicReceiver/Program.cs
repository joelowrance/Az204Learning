using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace ServiceBusTopicReceiver
{
    class Program
    {
        private static string TopicName = "MyTopic";
        private static string SubscriptionName = "Sub1";
        
        static async Task Main(string[] args)
        {   
            InitializeConfiguration();
            
            ServiceBusClient client = new ServiceBusClient(Configuration["ConnectionString"]);

            var rec = client.CreateReceiver(TopicName, SubscriptionName,
                new ServiceBusReceiverOptions() {ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete});

            var messages = await rec.ReceiveMessagesAsync(20);


            foreach (var message in messages)
            {
                Console.WriteLine(message.Body);
                
                foreach (var property in message.ApplicationProperties)
                {
                    Console.WriteLine($"{property.Key} -  {property.Value}");
                }
                
                Console.WriteLine("---");
            }
            
            

            Console.ReadKey();
        }
        
        #region config

        private static void InitializeConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<Program>();

            Configuration = builder.Build();
        }

        public static IConfigurationRoot Configuration { get; set; }

        #endregion
    }
}