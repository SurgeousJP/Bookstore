﻿using Basket.API.Middleware;
using Basket.API.Repositories;
using Basket.API.Services;
using Google.Api;
using Identity.API.Services;
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
            builder.Services.AddGrpc();
            builder.Services.AddGrpcSwagger();
            builder.Services.AddGrpcReflection();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT token with the prefix Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
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

            IConnectionMultiplexer redisServer =
                ConnectionMultiplexer.Connect(builder.Configuration["ConnectionStrings:Basket"]);

            builder.Services.AddSingleton(redisServer);
            builder.Services.AddSingleton<IBasketRepository, RedisBasketRepository>();
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddAuthorization();
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
