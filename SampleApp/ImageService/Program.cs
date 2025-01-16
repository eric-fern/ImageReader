using Azure;
//Azure AI imports
using Azure.AI.FormRecognizer.DocumentAnalysis;
using System;
using System.IO;
using System.Threading.Tasks;
/*
Present user with an image upload button.
User uploads image of ingredients label
Azure AI form recognizer provides an analysis about what you're eating and how healthy it is.
*/

class Program
{
    static async Task Main(string[] args)
    {
        string endpoint = "https://imagereaderinstance.cognitiveservices.azure.com/";
        string apiKey = "";
        string imagePath = "<path-to-your-image>";

        var credential = new AzureKeyCredential(apiKey);
        var client = new DocumentAnalysisClient(new Uri(endpoint), credential);

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