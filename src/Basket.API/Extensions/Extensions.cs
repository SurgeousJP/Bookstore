using Basket.API.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using StackExchange.Redis;

namespace Basket.API.Extensions
{
    public static class Extensions
    {
        public static void AddApplicationServices(this IHostApplicationBuilder builder)
        {
            builder.Services.AddGrpc();
            builder.Services.AddGrpcSwagger();
            builder.Services.AddGrpcReflection();

            builder.Services.AddSwaggerGen();

            IConnectionMultiplexer redisServer =
                ConnectionMultiplexer.Connect(builder.Configuration["ConnectionStrings:Basket"]);

            builder.Services.AddSingleton(redisServer);
            builder.Services.AddSingleton<IBasketRepository, RedisBasketRepository>();

            builder.Services.AddAuthentication(opts =>
            {
                opts.DefaultScheme =
                CookieAuthenticationDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme =
                CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/login"; 
            }); ;
            builder.Services.AddAuthorization();
        }
    }
}
