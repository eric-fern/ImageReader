using Microsoft.Extensions.Configuration; //lets us load up api keys and settings from an appsettings.json
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

        // Retrieve the endpoint and API key from the appsettings.json =
        string endpoint = config["FormRecognizer:Endpoint"];
        string apiKey = config["FormRecognizer:ApiKey"];
        //string imagePath = "<path-to-your-image>";

        // Create the Azure credentials and client
        var credential = new AzureKeyCredential(apiKey);
        var client = new DocumentAnalysisClient(new Uri(endpoint), credential);
        // Retrieve queue message 
        //pull blob url
        using var stream = new FileStream(imagePath, FileMode.Open);
        //overload version lets me provide a URL instead of a 
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

//CLAIM CHECK PATTERN (laundromat ticket)

//when user uploads,
//store image in blob in storage account
//insert message to queue saying we have a image ready to analyze - heres the blob location (url)

