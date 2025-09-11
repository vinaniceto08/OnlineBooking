using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using TravelTayo.Data;

var builder = WebApplication.CreateBuilder(args);

// Add authentication with B2C
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
    {
        builder.Configuration.Bind("AzureEntraB2C", options);

        options.Authority = $"{options.Instance}/{options.Domain}/v2.0/";
        options.MetadataAddress = $"{options.Instance}/{options.Domain}/v2.0/.well-known/openid-configuration?p={options.SignUpSignInPolicyId}";

    });

builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI(); // Provides ready-to-use login/logout endpoints

builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI(); // Adds identity support in Blazor pages
builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton<WeatherForecastService>();

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
