using System;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace ServiceBusReceiver
{
    class Program
    {
        private static string QueueName = "mainqueue";
        
        static void Main(string[] args)
        {
            InitializeConfiguration();
            
            ServiceBusClient client = new ServiceBusClient(Configuration["ConnectionString"]);
            
            
            //Peek lock will not remove the messages (locks for 30 seconds)
            var rec = client.CreateReceiver(QueueName,
                new ServiceBusReceiverOptions {ReceiveMode = ServiceBusReceiveMode.PeekLock});
            var messages = rec.ReceiveMessageAsync().GetAwaiter().GetResult();
            Console.WriteLine(messages.Body.ToString());
            // you have to delete manually
            rec.CompleteMessageAsync(messages).GetAwaiter().GetResult();
            
            // ReceiveAndDelete removes from queue
            // var rec2 = client.CreateReceiver(QueueName,
            //     new ServiceBusReceiverOptions {ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete});
            // var messages2 = rec.ReceiveMessageAsync().GetAwaiter().GetResult();

            Console.WriteLine(messages.Body.ToString());

            foreach (var property in messages.ApplicationProperties)
            {
                Console.WriteLine($"{property.Key} -  {property.Value}");
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