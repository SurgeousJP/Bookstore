using BookCatalog.API.Infrastructure;
using BookCatalog.API.Model;
using BookCatalog.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.API.Extensions
{
    public static class Extension
    {
        public static void AddApplicationServices(this IHostApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // Swagger genereator
            builder.Services.AddSwaggerGen();

            // Add repositories
            builder.Services.AddTransient<IRepository<Genre>, GenreRepository>();
            builder.Services.AddTransient<IRepository<Book>, BookRepository>();
            builder.Services.AddTransient<IRepository<BookFormat>, FormatRepository>();
            builder.Services.AddTransient<IRepository<BookPublisher>, PublisherRepository>();

            // Add Db context for catalog
            builder.Services.AddDbContext<BookContext>(options => {
                options.UseNpgsql(
                    builder.Configuration["ConnectionStrings:BookCatalog"
                    ]);
                options.EnableSensitiveDataLogging();
            });
        }
    }
}
