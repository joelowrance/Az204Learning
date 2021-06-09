using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Serialization;

namespace CosmosDb
{
    class Program
    {
        static async Task Main(string[] args)
        {
            InitializeConfiguration();
            
            var creator = new CreateDatabaseExample(Configuration);
            //await creator.Main();
                
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

    public class CreateDatabaseExample
    {
        private IConfigurationRoot _configurationRoot;

        public CreateDatabaseExample(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }


        public async Task Main()
        {
            var client = new CosmosClient(_configurationRoot["ConnectionString"]);
            var container = await client.CreateDatabaseAsync(Names.DatabaseName);

            var db = client.GetDatabase(Names.DatabaseName);
            var response = await db.CreateContainerAsync(Names.ContainerName, Names.PartitionKey);
            
            
            Console.WriteLine("Database and container have been created");
            
        }
        
    }

    public static class Names
    {
        public static readonly string DatabaseName = "MyDb";
        public static readonly string ContainerName = "Course";
        public static readonly string PartitionKey = "/courseid";
        
    }
}