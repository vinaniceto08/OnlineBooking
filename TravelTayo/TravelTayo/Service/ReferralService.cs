// Services/ReferralCodeService.cs
using Microsoft.JSInterop;

public class ReferralCodeService
{
    private readonly IJSRuntime _js;

    public ReferralCodeService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task SetReferralCodeAsync(string code)
    {
        await _js.InvokeVoidAsync("sessionStorage.setItem", "referralCode", code);
    }

    public async Task<string?> GetReferralCodeAsync()
    {
        return await _js.InvokeAsync<string?>("sessionStorage.getItem", "referralCode");
    }
}
