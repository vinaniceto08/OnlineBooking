using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using TravelTayo.Data;
using TravelTayo.Models;
using TravelTayo.Services;

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

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// Override password with env variable if present
var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();
emailSettings.Email_Password = Environment.GetEnvironmentVariable("Email_Password") ?? emailSettings.Email_Password;




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
