using Basket.API.Extensions;
using Basket.API.gRPC;
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

//app.UseHttpsRedirection();

app.MapGrpcService<BasketService>();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.Run();

