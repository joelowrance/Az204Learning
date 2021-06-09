using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace CosmosDb
{
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
}