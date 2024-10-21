using Basket.API.EventConsumer;
using Basket.API.Middleware;
using Basket.API.Repositories;
using Basket.API.Services;
using Identity.API.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Text;

namespace Basket.API.Extensions
{
    public static class Extensions
    {
        public static void AddApplicationServices(this IHostApplicationBuilder builder)
        {
            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket", Version = "v1" });
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
            });
            var redisConfig = new ConfigurationOptions
            {
                EndPoints = { builder.Configuration["ConnectionStrings:Basket"] },
                Password = builder.Configuration["ConnectionStrings:Password"],
                AbortOnConnectFail = false,
                ConnectRetry = 10,  // Retries before giving up
                ConnectTimeout = 20000  // Timeout in milliseconds
            };
            IConnectionMultiplexer redisServer =
                ConnectionMultiplexer.Connect(redisConfig);

            builder.Services.AddSingleton(redisServer);
            builder.Services.AddSingleton<IBasketRepository, RedisBasketRepository>();

            builder.Services.AddAuthorization();
            builder.Services.AddControllers();

            builder.Services.AddMassTransit(config =>
            {
                config.AddConsumer<ProductPriceUpdateEventConsumer>();
                config.AddConsumer<OrderStartedEventConsumer>();

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);

                    cfg.ReceiveEndpoint(EventBus.Messaging.EventBusConstant.EventBusConstants.ProductUpdateQueue, c =>
                    {
                        c.ConfigureConsumer<ProductPriceUpdateEventConsumer>(ctx);
                    });

                    cfg.ReceiveEndpoint(EventBus.Messaging.EventBusConstant.EventBusConstants.OrderStartedQueue, c =>
                    {
                        c.ConfigureConsumer<OrderStartedEventConsumer>(ctx);
                    });
                });
            });
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
}
