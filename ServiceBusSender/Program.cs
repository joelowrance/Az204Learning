using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Transactions;
using System.Windows.Markup;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using CommonSupport;


namespace ServiceBusSender
{
    class Program
    {
        private static string QueueName = "mainqueue";
        
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
            Azure.Messaging.ServiceBus.ServiceBusSender sender = client.CreateSender(QueueName);

            foreach (var order in orders)
            {
                var message = new ServiceBusMessage(order.ToString())
                {
                    ContentType = "application/json",
                    TimeToLive = TimeSpan.FromSeconds(30), // can override the queue default
                };
                
                message.ApplicationProperties.Add("Department", "Sales");
                
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