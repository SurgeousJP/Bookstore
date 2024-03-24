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

app.Run();
