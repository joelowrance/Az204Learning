using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace BlobStorage
{
    public static class Names
    {
        public const string ContainerName = "samplecontainer";
    }
    
    class Program
    {
        static async Task Main(string[] args)
        {   
            InitializeConfiguration();
            
            //create a container
            var client = new BlobServiceClient(Configuration["ConnectionString"]);
            var container = await client.CreateBlobContainerAsync(Names.ContainerName, PublicAccessType.BlobContainer);

            // upload a file
            var refer = client.GetBlobContainerClient(Names.ContainerName);
            await refer.UploadBlobAsync("blob.jpg", BinaryData.FromBytes(await File.ReadAllBytesAsync(@"c:\temp\IMG_4419.jpeg")));
            
            // or much easier
            var uploadClient = refer.GetBlobClient(Names.ContainerName);
            await uploadClient.UploadAsync(@"c:\temp\IMG_4419.jpeg");
            
            //list blobs
            var list =  refer.GetBlobsAsync();
            await foreach (var f in list)
            {
                Console.WriteLine(f.Name);
            }
            
            // download
             var blobClient = refer.GetBlobClient("blob.jpg");
             var downloaded = await blobClient.DownloadToAsync(@"c:\temp\yay.jpg");
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