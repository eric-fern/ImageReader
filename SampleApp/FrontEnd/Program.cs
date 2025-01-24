using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FrontEnd;
using System.Net.Http;

/*
This is a hosted Blazor WebAssembly app. 
Using hosted Blazor WebAssembly,  .NET, including the ability to share code between the client and server apps, support for prerendering, and integration with MVC and Razor Pages. 
This hosted client app can interact with 
its backend server app over the network using a variety of messaging frameworks and protocols, 
such as web API, gRPC-web, and SignalR (Use ASP.NET Core SignalR with Blazor).

for sending images to 
*/


var builder = WebAssemblyHostBuilder.CreateDefault(args);
//our app
builder.RootComponents.Add<App>("#app");

builder.RootComponents.Add<HeadOutlet>("head::after");


/*Create http client for user
AddScoped makes sure that the same instance of HttpClient is used for the lifetime of a user session. Avoids bugs down the road. This same
http client can be injected into our components and services.

sp is our service provider. Its not actually useful or necessary here, but it is a parameter that is required by the AddScoped method.

upon instantiation configure a base address from host environment. This lets you use relative urls in http requests instead of absolute urls.

 */
builder.Services.AddScoped<HttpClient>(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


await builder.Build().RunAsync();

