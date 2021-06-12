using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CosmosDb
{
    class Program
    {
        static async Task Main(string[] args)
        {
            InitializeConfiguration();
         
            // need to comment and uncomment as you go.
            //var creator = new CreateDatabaseExample(Configuration);
            //await creator.Main();

            var workingWithItems = new WorkingWithItemsExample(Configuration);
            //await workingWithItems.AddItemToDb();
            //await workingWithItems.BulkAddItems();
            //await workingWithItems.ReadItems();
            //await workingWithItems.UpdateItems();
            //await workingWithItems.DeleteItem();
                
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

    public static class Names
    {
        public static readonly string DatabaseName = "MyDb";
        public static readonly string ContainerName = "Course";
        public static readonly string PartitionKey = "/courseid";
        
    }

    public class Course
    {
        
        //database id
        [JsonProperty("id")]
        public string Id { get; set; }
        
        // courseid is the partition key
        [JsonProperty("courseid")]
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal Rating { get; set; }
    }
}