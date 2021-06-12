using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;

namespace CosmosDbTableApi
{
    class Program
    {
        // THIS IS THE ALMOST THE EXACT SAME CODE AS THE TABLE STORAGE (table name is diff) 
        // different connection string.
        static async Task Main(string[] args)
        {
            InitializeConfiguration();
            
            var cla = CloudStorageAccount.Parse(Configuration["ConnectionString"]);
            var client = cla.CreateCloudTableClient();
            
            var table = client.GetTableReference("customer");


            // Create a customer
            var customer = new Customer("Joe", "New York", "1");
            var op = TableOperation.Insert(customer);
            await table.ExecuteAsync(op); 
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
    
    public class Customer : TableEntity
    {
        public string CustomerName { get; set; }

        public Customer(string name, string city, string customerId)
        {
            PartitionKey = city;
            RowKey = customerId;
            CustomerName = name;
        }

        //Needs empty ctor to retreive.
        public Customer()
        {
            
        }
    }
}