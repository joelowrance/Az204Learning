using System;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Channels;
using CommonSupport;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Order = CommonSupport.Order;

namespace AzureCacheForRedisSimple
{
    class Program
    {
        private static readonly Lazy<ConnectionMultiplexer> CacheConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            var connString = Configuration["Redis"];
            return ConnectionMultiplexer.Connect(connString);
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return CacheConnection.Value;
            }
        }
        
        static void Main(string[] args)
        {
            InitializeConfiguration();

            
            // Simple String
            var db = Connection.GetDatabase();
            if (db.KeyExists("StringValue"))
            {
                string stringValue = db.StringGet("StringValue");    
                Console.WriteLine(stringValue);
            }
            else
            {
                db.StringSet("StringValue", "A Simple string value");
                Console.WriteLine("Added key to cache");
            }
            
            // Regular class (uses StringSet but needs JsonSerialize)
            var order = new Order {Orderid = "100", Quantity = 10, UnitPrice = 9.99m};
            db.StringSet(order.Orderid, JsonSerializer.Serialize(order));
            var fromCache = JsonSerializer.Deserialize<Order>(db.StringGet(order.Orderid));
            Console.WriteLine(fromCache);


            Console.ReadKey();

        }
        
        #region config

        private static void InitializeConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<Program>();

            var cfg = builder.Build();
            Configuration = cfg;
        }

        public static IConfigurationRoot Configuration { get; set; }

        #endregion
    }
}