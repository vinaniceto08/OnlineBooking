using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class LoginModel : PageModel
{
    public IActionResult OnGet(string? returnUrl = "/")
    {
        // trigger Entra B2C challenge
        return Challenge(
            new AuthenticationProperties { RedirectUri = returnUrl },
            OpenIdConnectDefaults.AuthenticationScheme);
    }
}
