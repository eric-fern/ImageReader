using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Queues;
using Azure.Storage.Blobs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly QueueClient _queueClient;
    private readonly BlobServiceClient _blobServiceClient;

    //Constructor injection
    //UploadController receives instance of our queueclient and blobclient through its constructor
    public UploadController(QueueClient queueClient, BlobServiceClient blobServiceClient)
    {   
        //nicknames for our client singletons so we can use them in the controller
        _queueClient = queueClient;
        _blobServiceClient = blobServiceClient;
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        // Upload the file to Blob Storage
        var blobContainer = _blobServiceClient.GetBlobContainerClient("uploads");
        var blobClient = blobContainer.GetBlobClient(file.FileName);
        await using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, true);

        // Create a message with the file URL and add it to the queue
        var message = new { BlobUrl = blobClient.Uri.ToString() };
        await _queueClient.SendMessageAsync(JsonConvert.SerializeObject(message));

        return Ok("File uploaded successfully.");
    }
}
