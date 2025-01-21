## Azure AI form recognizer provides an analysis about what you're eating and how healthy it is.


- Present user with an image upload button.
- User uploads image of ingredients label
- Web client sends HTTP POST of the image to our console app (Wrapper for Azure AI Form Analyzer API)
- Our Console App Wrapper passes the image to the Form service, makes sure we received only image data, hides our API key, and load limits our service. 
- Images to be proceessed are stored in a queue. It will be a queue of URLs (URLs point to blob image storage on cloud).

```mermaid
sequenceDiagram
participant User
participant Frontend as Frontend (Blazor WASM)
participant Backend as Backend Service Wrapper
participant Azure as Azure Form Recognizer

User->>Frontend: Select image file
Frontend->>Frontend: Process file in ImageUpload.razor
Frontend->>Backend: Send image file wit http post request
Backend->>Backend:Put image in a queue (queue of URLs to be processed)
Backend->>Backend:Rate limit and only accept images to form analyzer
Backend->>Backend:Hide our API keys and increase security. Avoid Cross-Origin Resource sharing (CORS), 
Backend->>Azure: Iterate over queue, send images to be analyzed with a system prompt.
Azure->>Backend: Return analysis results
Backend->>Frontend: Return formatted results
Frontend->>User: Display results
```
