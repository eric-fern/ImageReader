## Azure AI form recognizer provides an analysis about what you're eating and how healthy it is.


- Present user with an image upload button.
- User uploads image of ingredients label
- Web client/ios/android sends HTTP POST of the image to our Web API App (Wrapper for Azure AI Form Analyzer API)
- Our Console App Wrapper passes the image to the Form service, makes sure we received only image data, hides our API key, and load limits our service. 
- Images to be proceessed are stored in a queue. It will be a queue of URLs (URLs point to Azure blob storage).


- Can easily add a macros visual calculator similar to CalAI, the AI might also hold your macros in context for future ingredients labels you're scanning.
- The reason is, you might have hit your target fat, sugar, carbs or protein intake for the day, and you want to notify the user and store the data.
```mermaid

sequenceDiagram
participant User
participant Frontend as Frontend (Blazor WASM)
participant Backend as Backend Service Wrapper
participant Azure as Azure Form Recognizer

User->>Frontend: Select image file
Frontend->>Frontend: Process file in ImageUpload.razor
Frontend->>Backend: Send image file wit http post request
Backend->>Backend:Put image in a queue (queue of URLs pointing at BLOB images to be processed)
Backend->>Backend:Rate limit and only accept images to form analyzer
Backend->>Backend:Hide our API keys and increase security. Avoid Cross-Origin Resource sharing (CORS), 
Backend->>Azure: Iterate over queue, send images to be analyzed with a system prompt.
Azure->>Backend: Return analysis results
Backend->>Frontend: Return formatted results
Frontend->>User: Display results
```
