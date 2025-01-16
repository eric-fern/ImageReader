﻿using Microsoft.Extensions.Configuration; //lets us load up api keys and settings from an appsettings.json
using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis; //Azure AI imports
using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
/*
Present user with an image upload button.
User uploads image of ingredients label
Azure AI form recognizer provides an analysis about what you're eating and how healthy it is.
This is the backend which hits an API with your image and gets the analysis back to send to the front end
The front end sends their uploaded image via HTTP POST request to this image service right here.
*/

class Program
{
    static async Task Main(string[] args)
    {
        //go to our appsettings.json and assign apiKey and endpoint

        // Create a ConfigurationBuilder instance
        var config = new ConfigurationBuilder()
            // Set the base path to the current directory
            .SetBasePath(Directory.GetCurrentDirectory())
            // Add the appsettings.json file
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            // Build the configuration. Ends up creating an IConfigurationRoot object which is used to access our json stuff
            .Build();

        // Retrieve the endpoint and API key from the configuration
        string endpoint = config["FormRecognizer:Endpoint"];
        string apiKey = config["FormRecognizer:ApiKey"];
        string imagePath = "<path-to-your-image>";

        // Create the Azure credentials and client
        var credential = new AzureKeyCredential(apiKey);
        var client = new DocumentAnalysisClient(new Uri(endpoint), credential);
        // Open the image file
        using var stream = new FileStream(imagePath, FileMode.Open);

        var operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-read", stream);
        var result = operation.Value;

        foreach (var page in result.Pages)
        {
            foreach (var line in page.Lines)
            {
                Console.WriteLine(line.Content);
            }
        }
    }
}