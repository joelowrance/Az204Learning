using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Transactions;
using System.Windows.Markup;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

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
                var message = new ServiceBusMessage(order.ToString());
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

    class Order
    {
        public string Orderid  { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice  { get; set; }

        public override string ToString()
        {
            //return $"OrderId: {Orderid}, Qty: {Quantity}, Price {UnitPrice:C}";
            return JsonSerializer.Serialize(this);
        }
    }
    
    
}