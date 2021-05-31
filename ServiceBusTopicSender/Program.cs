using System;
using System.Collections.Generic;
using Azure.Messaging.ServiceBus;
using CommonSupport;
using Microsoft.Extensions.Configuration;

namespace ServiceBusTopicSender
{
    class Program
    {
        //Only change in the sender is we send to a topic.
        private static string TopicName = "MyTopic";
        
        static void Main(string[] args)
        {
            InitializeConfiguration();
            
            var orders = new List<Order>()
            {
                new Order {Orderid = "O1", Quantity = 10, UnitPrice = 9.99m},
                new Order {Orderid = "O2", Quantity = 11, UnitPrice = 19.99m},
                new Order {Orderid = "O3", Quantity = 12, UnitPrice = 39.99m},
                new Order {Orderid = "O4", Quantity = 13, UnitPrice = 29.99m},
                new Order {Orderid = "O5", Quantity = 14, UnitPrice = 19.99m},
            };
            
            

            ServiceBusClient client = new ServiceBusClient(Configuration["ConnectionString"]);
            Azure.Messaging.ServiceBus.ServiceBusSender sender = client.CreateSender(TopicName);

            foreach (var order in orders)
            {
                var message = new ServiceBusMessage(order.ToString())
                {
                    ContentType = "application/json",
                    TimeToLive = TimeSpan.FromSeconds(300), // BE CAREFUL!!!
                };
                
                message.ApplicationProperties.Add("Topic", "Yes");
                
                sender.SendMessageAsync(message).GetAwaiter().GetResult();
                
            }
            
            Console.WriteLine("All messages sent");
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