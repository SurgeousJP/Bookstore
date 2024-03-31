using Basket.API.Extensions;
using Basket.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseMiddleware<JwtMiddleware>(); // JWT Middleware

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

