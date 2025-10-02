using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using TravelTayo.Data;
using TravelTayo.Interfaces;
using TravelTayo.Models;
using TravelTayo.Services;
using TravelTayo.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add authentication with B2C
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
    {
        // Bind non-sensitive config from appsettings.json
        builder.Configuration.Bind("AzureEntraB2C", options);

        // Override sensitive info from environment variables
        options.ClientId = Environment.GetEnvironmentVariable("B2C_CLIENT_ID")
                   ?? builder.Configuration["AzureEntraB2C:ClientId"];

        //options.ClientSecret = Environment.GetEnvironmentVariable("B2C_CLIENT_SECRET") // if needed
        //                        ?? builder.Configuration["AzureEntraB2C:ClientSecret"];


        // Authority and Metadata
        options.Authority = $"{options.Instance}/{options.Domain}/v2.0/";
        options.MetadataAddress = $"{options.Instance}/{options.Domain}/v2.0/.well-known/openid-configuration?p={options.SignUpSignInPolicyId}";

        options.TokenValidationParameters.NameClaimType = "name";      // for display purposes
         


    });
string connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING")
                          ?? builder.Configuration.GetConnectionString("TravelTayoDb");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddTransient<EmailService>();
builder.Services.AddScoped<IReferralService, ReferralService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ReferralCodeService>();

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// Override password with env variable if present
var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();
emailSettings.Email_Password = Environment.GetEnvironmentVariable("Email_Password") ?? emailSettings.Email_Password;

// Load static blob config
BlobConfig.Load(builder.Configuration);


builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI(); // Provides ready-to-use login/logout endpoints

builder.Services.AddControllers();
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI(); // Adds identity support in Blazor pages
builder.Services.AddServerSideBlazor();
builder.Services.AddAuthorizationCore();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<AuthenticationStateProvider, FakeAuthenticationStateProvider>();
}


var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
// security protection
app.Use(async (context, next) =>
{
    // Security headers
    context.Response.Headers.TryAdd("X-Content-Type-Options", "nosniff");
    context.Response.Headers.TryAdd("Referrer-Policy", "no-referrer");
    context.Response.Headers.TryAdd("X-Frame-Options", "DENY");


    // Remove technology disclosure headers (server fingerprinting)
    context.Response.Headers.Remove("Server");
    context.Response.Headers.Remove("X-Powered-By");
    context.Response.Headers.Remove("X-AspNet-Version");
    context.Response.Headers.Remove("X-AspNetMvc-Version");

    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable auth middlewares
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // Needed for Identity UI
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
