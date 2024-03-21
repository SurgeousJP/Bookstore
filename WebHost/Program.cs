using Basket.API.gRPC;
using Grpc.Core;
using Grpc.Net.Client;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath);

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();
await app.UseOcelot();

var channel = GrpcChannel.ForAddress("http://localhost:5183");
var client = new Basket.API.gRPC.Basket.BasketClient(channel);
var request = new GetBasketRequest();

var response = await client.GetBasketAsync(request, new CallOptions());

Console.WriteLine(response);

app.Run();
