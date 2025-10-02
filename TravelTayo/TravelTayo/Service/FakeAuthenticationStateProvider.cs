using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

public class FakeAuthenticationStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal _user;

    public FakeAuthenticationStateProvider()
    {
        // Default to a fake authenticated user
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "marvin aniceto"),
            new Claim(ClaimTypes.NameIdentifier, "dev-user-id"),
            new Claim("email", "vinaniceto08@gmail.com"),
        }, "FakeAuth");

        _user = new ClaimsPrincipal(identity);
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(_user));
    }

    public void SetUser(ClaimsPrincipal user)
    {
        _user = user;
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_user)));
    }
}
