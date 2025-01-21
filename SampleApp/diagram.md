sequenceDiagram
    participant User
    participant Frontend as Frontend (Blazor WASM)
    participant Backend as Backend (API)
    participant Azure as Azure Form Recognizer

    User->>Frontend: Select image file
    Frontend->>Frontend: Process file in<br/>ImageUpload.razor
    Frontend->>Backend: HTTP POST with<br/>MultipartFormDataContent
    Backend->>Azure: Send image for processing
    Azure->>Backend: Return extracted text/analysis
    Backend->>Frontend: Return results
    Frontend->>User: Display results