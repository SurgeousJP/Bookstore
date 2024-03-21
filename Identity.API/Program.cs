using Identity.API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Identity.API.Extensions;
using Identity.API.Services;
using Identity.API.Models;

var builder = WebApplication.CreateBuilder(args);

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
    opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
});

builder.Services.AddDbContext<IdentityContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration["ConnectionStrings:IdentityConnection"]);
    opts.EnableSensitiveDataLogging();
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>();
builder.Services.AddTransient<ILoginService<ApplicationUser>, LoginService>();
builder.Services.AddTransient<IProfileService, ProfileService>();

builder.Services.AddAuthentication(opts => {
    opts.DefaultScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    opts.DefaultChallengeScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(opts => {
    opts.Events.DisableRedirectForPath(e => e.OnRedirectToLogin,
    "/api", StatusCodes.Status401Unauthorized);
    opts.Events.DisableRedirectForPath(e => e.OnRedirectToAccessDenied,
    "/api", StatusCodes.Status403Forbidden);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();
app.MapRazorPages();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
