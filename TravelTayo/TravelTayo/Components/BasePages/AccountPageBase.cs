using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using TravelTayo.Interfaces;
using TravelTayo.Models;

namespace TravelTayo.Shared.BasePages
{
    public class AccountPageBase : ComponentBase
    {
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject] protected IAccountService AccountService { get; set; }
        [Inject] protected NavigationManager Navigation { get; set; }

        protected Account AccountModel { get; set; } = new();
        protected ClaimsPrincipal User { get; set; }

        /// <summary>
        /// If true, page requires the account to have ContactNumber filled.
        /// If false, the page is public and will fetch account if logged in but won't redirect.
        /// </summary>
        protected bool RequireCompleteAccount { get; set; } = true;

        protected async Task InitializeAccountAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            User = authState.User;

            if (!(User.Identity?.IsAuthenticated ?? false))
            {
                // Only redirect if account is required
                if (RequireCompleteAccount)
                    Navigation.NavigateTo("/authentication/login");
                return;
            }

            var b2cUserId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (!string.IsNullOrEmpty(b2cUserId))
            {
                var existingAccount = await AccountService.GetByB2CUserIdAsync(b2cUserId);

                if (existingAccount != null)
                {
                    AccountModel = existingAccount;

                    // Redirect to account page if required and contact number is missing
                    if (RequireCompleteAccount && string.IsNullOrEmpty(existingAccount.ContactNumber))
                    {
                        Navigation.NavigateTo("/account");
                        return;
                    }
                }
                else
                {
                    // New account
                    AccountModel.B2CUserId = b2cUserId;
                    AccountModel.FullName = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? "";
                    AccountModel.Email = User.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value ?? "";
                }
            }
        }

        protected async Task SaveAccountAsync()
        {
            if (AccountModel.AccountId == Guid.Empty)
            {
                await AccountService.AddOrUpdateAsync(AccountModel);
            }
            else
            {
                await AccountService.AddOrUpdateAsync(AccountModel);
            }
        }
    }
}
