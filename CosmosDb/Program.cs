﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Microsoft.Azure.Cosmos;
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

    public class WorkingWithItemsExample
    {
        private IConfigurationRoot _configurationRoot;

        public WorkingWithItemsExample(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }
        
        public async Task AddItemToDb()
        {
            var client = new CosmosClient(_configurationRoot["ConnectionString"]);

            var course = new Course
            {
                
                Rating = 1.1m,
                CourseId = "Course1",
                CourseName = "Course Name 1",
                Id = "1"
            };
            
            var container = client.GetContainer(Names.DatabaseName, Names.ContainerName);
            await container.CreateItemAsync<Course>(course, new PartitionKey(course.CourseId));
        }
        
        public async Task BulkAddItems()
        {
            var client = new CosmosClient(_configurationRoot["ConnectionString"], new CosmosClientOptions
            {
                AllowBulkExecution = true
            });

            var courses = new List<Course>
            {
                new Course {Rating = 1.2m, CourseId = "Course2", CourseName = "Course Name 2", Id = "2"},
                new Course {Rating = 1.3m, CourseId = "Course3", CourseName = "Course Name 3", Id = "3"},
                new Course {Rating = 1.4m, CourseId = "Course4", CourseName = "Course Name 4", Id = "4"},
                new Course {Rating = 1.5m, CourseId = "Course5", CourseName = "Course Name 5", Id = "5"},
                new Course {Rating = 1.6m, CourseId = "Course6", CourseName = "Course Name 6", Id = "6"},
            };
            
            var container = client.GetContainer(Names.DatabaseName, Names.ContainerName);
            var tasks = new List<Task>();
            courses.ForEach(x =>
            {
                tasks.Add(container.CreateItemAsync<Course>(x, new PartitionKey(x.CourseId)));
            });


            await Task.WhenAll(tasks);
            
            Console.WriteLine("All items added");
        }

        public async Task ReadItems()
        {
            var client = new CosmosClient(_configurationRoot["ConnectionString"]);
            var container = client.GetContainer(Names.DatabaseName, Names.ContainerName);

            var def = new QueryDefinition("SELECT * FROM c where c.courseid='Course1'");
            var iterator = container.GetItemQueryIterator<Course>(def);

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();

                foreach (var course in response)
                {
                    Console.WriteLine(course.Rating);
                }
            }
        }
        
        public async Task UpdateItems()
        {
            var client = new CosmosClient(_configurationRoot["ConnectionString"]);
            var container = client.GetContainer(Names.DatabaseName, Names.ContainerName);

            var response =  await container.ReadItemAsync<Course>("1", new PartitionKey("Course1"));
            var course = response.Resource;
            course.Rating = 4.5m;

            await container.ReplaceItemAsync<Course>(course, course.Id, new PartitionKey(course.CourseId));
        }


        public async Task DeleteItem()
        {
            var client = new CosmosClient(_configurationRoot["ConnectionString"]);
            var container = client.GetContainer(Names.DatabaseName, Names.ContainerName);
            var result = await container.DeleteItemAsync<Course>("1", new PartitionKey("Course1"));
            
            Console.WriteLine(result.Resource.CourseName + " was deleted");
        }
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