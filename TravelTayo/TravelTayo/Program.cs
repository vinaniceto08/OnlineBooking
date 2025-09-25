using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using TravelTayo.Data;
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

        options.TokenValidationParameters.NameClaimType = "name";
        options.TokenValidationParameters.NameClaimType = "emails";

    });
string connectionString = builder.Configuration.GetConnectionString("TravelTayoDb");

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

//agoda api
builder.Services.AddHttpClient<AgodaService>();
builder.Services.AddScoped<AgodaService>(sp =>
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    var db = sp.GetRequiredService<AppDbContext>();
    var apiKey = builder.Configuration["Agoda:ApiKey"];
    return new AgodaService(httpClient, db, apiKey);
});


builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI(); // Provides ready-to-use login/logout endpoints

builder.Services.AddControllers();
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI(); // Adds identity support in Blazor pages
builder.Services.AddServerSideBlazor();



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
