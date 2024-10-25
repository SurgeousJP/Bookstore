using Hangfire;
using Identity.API.Data;
using Identity.API.Extensions;
using Identity.API.Models;
using Identity.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddJwt(configuration);
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.Configure<IdentityOptions>(opts =>
{
    opts.Password.RequiredLength = 8;
    opts.Password.RequireNonAlphanumeric = true;
    opts.Password.RequireLowercase = true;
    opts.Password.RequireUppercase = true;
    opts.Password.RequireDigit = true;

    opts.User.RequireUniqueEmail = true;
    opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
});

var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();

builder.Services.AddSingleton<EmailConfiguration>(emailConfig);

builder.Services.AddDbContext<IdentityContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration["ConnectionStrings:IdentityConnection"]);
    opts.EnableSensitiveDataLogging();
});

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders();
builder.Services.AddTransient<ILoginService<ApplicationUser>, LoginService>();
builder.Services.AddTransient<IProfileService, ProfileService>();
builder.Services.AddEndpointsApiExplorer();

// Configure Hangfire with SQL Server for production
builder.Services.AddHangfire(configuration => configuration
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseInMemoryStorage());
builder.Services.AddHangfireServer();

var app = builder.Build();

app.MapAPI_File();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseHangfireDashboard();
app.MapControllers();
app.MapHangfireDashboard();
app.MapRazorPages();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
