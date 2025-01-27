using Microsoft.Extensions.Configuration; //lets us load up api keys and settings from an appsettings.json
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;


using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis; //Azure AI imports
using Azure.Storage.Queues;
using Azure.Storage.Blobs;

using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;

/*
The front end sends their uploaded image via HTTP POST request to this image service right here.
Configuration builder is for our json appsettings
Host is for Configuring our services
We need a service for handling blob storage, queue storage, form recognizerm, apicontroller.
WebHost lets us listen for HTTP requests

Changed csproj to Web project 
*/


class Program
{
    static async Task Main(string[] args)
    {
        //Create our appsettings.json
        // Create a ConfigurationBuilder instance
        var config = new ConfigurationBuilder()
            // Set the base path to the current directory
            .SetBasePath(Directory.GetCurrentDirectory())
            // Add the appsettings.json file
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            // Build the configuration. Ends up creating an IConfigurationRoot object which is used to access our json stuff
            .Build();

        string[] requiredSettings = new[]
{
            "BlobStorage:ConnectionString",
            "QueueStorage:ConnectionString",
            "QueueStorage:QueueName",
            "FormRecognizer:Endpoint",
            "FormRecognizer:ApiKey"
        };

        foreach (var setting in requiredSettings)
        {
            if (string.IsNullOrEmpty(config[setting]))
            {
                Console.WriteLine($"{setting} is required.");
                return;
            }
        }

        // Create a HostBuilder instance
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                //pull all this stuff from our appsettings
                //Retrieve configuration settings
                //config[] pattern gives us access to our IConfigurationRoot object which lets us access our json stuff
                string blobConnectionString = config["BlobStorage:ConnectionString"];
                string queueConnectionString = config["QueueStorage:ConnectionString"];
                string queueName = config["QueueStorage:QueueName"];
                //our connection details to FormRecognizer
                string endpoint = config["FormRecognizer:Endpoint"];
                string apiKey = config["FormRecognizer:ApiKey"];

                // Using our appsettings.json stuff that we pulled using conig[], we can set up our services (Singletons)
                services.AddSingleton(new BlobServiceClient(blobConnectionString));
                services.AddSingleton(new QueueClient(queueConnectionString, queueName));
                services.AddSingleton(new DocumentAnalysisClient(new Uri(endpoint), new AzureKeyCredential(apiKey)));
                //Add our API Controllers
                services.AddControllers();
            })

            //we need to configure a webserver to listen for incoming HTTP requests
             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.Configure(app =>
                 {
                     //using routing and endpoint mapping to route requests to the proper controller
                     app.UseRouting();

                     app.UseEndpoints(endpoints =>
                     {
                         endpoints.MapControllers(); // Add this line
                     });
                 });
             })

            .Build();


        // Run the host
        await host.RunAsync();
    }

}

//CLAIM CHECK PATTERN (laundromat ticket)

//when user uploads,
//store image in blob in storage account
//insert message to queue saying we have a image ready to analyze - heres the blob location (url)

