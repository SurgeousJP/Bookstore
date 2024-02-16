using BookCatalog.API.Infrastructure;
using BookCatalog.API.Model;
using BookCatalog.API.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers()
    .AddNewtonsoftJson(); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IRepository<Genre>, GenreRepository>();
builder.Services.AddTransient<IRepository<Book>, BookRepository>();
builder.Services.AddTransient<IRepository<BookFormat>, FormatRepository>();

builder.Services.AddDbContext<BookContext>(options => {
    options.UseNpgsql(
        builder.Configuration["ConnectionStrings:BookCatalog"
        ]);
    options.EnableSensitiveDataLogging();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
