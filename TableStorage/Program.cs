using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;

namespace TableStorage
{
    class Program
    {
        private static string TableName = "Customers";
        private static string ConnectionString => Configuration["ConnectionString"];
        
        static async Task Main(string[] args)
        {
            InitializeConfiguration();

            var cla = CloudStorageAccount.Parse(ConnectionString);
            var client = cla.CreateCloudTableClient();
            
            var table = client.GetTableReference(TableName);
            await table.CreateIfNotExistsAsync();
            Console.WriteLine("Created");

            // Create a customer
            var customer = new Customer("Joes", "New York", "1");
            var op = TableOperation.Insert(customer);
            await table.ExecuteAsync(op); 
            
            // create a  bunch of customers, part. key neds to be same.
            // var customerList = new List<Customer>
            // {
            //     new Customer("Thomas", "Severn", "2"),
            //     new Customer("Amber", "Severn", "3")
            // };
            //
            // var batchOperation = new TableBatchOperation();
            // customerList.ForEach(x => batchOperation.Insert(x));
            // await table.ExecuteBatchAsync(batchOperation);
            
            //read 
            var op2 = TableOperation.Retrieve<Customer>("Severn", "1");
            var customerResult = await table.ExecuteAsync(op);
            var customer2 = customerResult.Result as Customer;
            Console.WriteLine(customer2.CustomerName);
            
            //update
            customer.CustomerName = customer2.CustomerName + DateTime.Now.Ticks.ToString();
            var op3 = TableOperation.InsertOrReplace(customer2);
            await table.ExecuteAsync(op3);
            
            //Delete
            var op4 = TableOperation.Delete(customer2);
            await table.ExecuteAsync(op4);
            
            
            
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