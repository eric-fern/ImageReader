sequenceDiagram
    participant User
    participant Frontend as Frontend (Blazor WASM)
    participant Backend as Backend Service Wrapper
    participant Azure as Azure Form Recognizer

    User->>Frontend: Select image file
    Frontend->>Frontend: Process file in<br/>ImageUpload.razor
    Frontend->>Backend: Send image file
    Note over Backend: Handle authentication<br/>and request formatting
    Backend->>Azure: Forward processed request
    Azure->>Backend: Return analysis results
    Note over Backend: Format response data
    Backend->>Frontend: Return formatted results
    Frontend->>User: Display results
