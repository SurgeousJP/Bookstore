using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath);
builder.Configuration.AddJsonFile("ocelot.json");

builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerForOcelot(builder.Configuration);
builder.Services.AddSwaggerGen();
var app = builder.Build();

app.UseSwaggerForOcelotUI(option =>
{
    option.PathToSwaggerGenerator = "/swagger/docs";
});
app.UseHttpsRedirection();
app.UseOcelot().Wait();

app.MapControllers();
app.Run();
