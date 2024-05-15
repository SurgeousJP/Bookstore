using BookCatalog.API.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ordering.API.EventConsumer;
using Ordering.API.Infrastructure;
using Ordering.API.Middleware;
using Ordering.API.Model;
using Ordering.API.Repositories;
using Ordering.API.Services;
using Stripe;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ordering.API.Extensions
{
    public static class Extension
    {
        public static void AddApplicationServices(this IHostApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddMvc().AddNewtonsoftJson();


            // Swagger genereator
            builder.Services.AddSwaggerGen(c =>
            {
                c.MapType<DateOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "date"
                });
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT token with the prefix Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
            }); ;

            builder.Services.AddScoped<IBuyerRepository, BuyerRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();

            // Add Db context for catalog
            builder.Services.AddDbContext<OrderContext>(options =>
            {
                options.UseNpgsql(
                    builder.Configuration["ConnectionStrings:Ordering"
                    ]);
                options.EnableSensitiveDataLogging();
            });

            builder.Services.AddMassTransit(config =>
            {
                config.AddConsumer<OrderStatusChangedToStockedCancelEventConsumer>();
                config.AddConsumer<OrderStatusChangedToStockedConfirmedEventConsumer>();

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);

                    cfg.ReceiveEndpoint(EventBus.Messaging.EventBusConstant.EventBusConstants.OrderStatusChangedToStockCancelQueue, c =>
                    {
                        c.ConfigureConsumer<OrderStatusChangedToStockedCancelEventConsumer>(ctx);
                    });

                    cfg.ReceiveEndpoint(EventBus.Messaging.EventBusConstant.EventBusConstants.OrderStatusChangedToStockConfirmedQueue, c =>
                    {
                        c.ConfigureConsumer<OrderStatusChangedToStockedConfirmedEventConsumer>(ctx);
                    });
                });
            });

            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));
            StripeConfiguration.ApiKey = builder.Configuration["StripeSettings:StripeAPIKey"];
        }

        public static void AddJwtAuthentication
    (this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("jwt");
            var options = section.Get<JwtOptions>();
            var key = Encoding.UTF8.GetBytes(options.Secret);
            section.Bind(options);
            services.Configure<JwtOptions>(section);

            services.AddSingleton<IJwtBuilder, JwtBuilder>();
            services.AddTransient<JwtMiddleware>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(x =>
            {
                x.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });
        }
    }
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private const string Format = "yyyy-MM-dd";

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateOnly.ParseExact(reader.GetString(), Format, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
        }
    }
}
