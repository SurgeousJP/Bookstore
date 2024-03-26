using Basket.API.Extensions;
using Basket.API.gRPC;
using Basket.API.Middleware;
using Grpc.Core;
using Grpc.Net.Client;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGrpcService<BasketService>();

app.UseRouting();

app.UseMiddleware<JwtMiddleware>(); // JWT Middleware
app.UseAuthentication();
app.UseAuthorization();

app.Run();

