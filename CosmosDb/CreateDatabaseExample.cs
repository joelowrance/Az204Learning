using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace CosmosDb
{
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
}