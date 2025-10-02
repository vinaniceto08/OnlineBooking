using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelTayo.Data;
using TravelTayo.Models;

public interface IReferralService
{
    Task<CheckUserResponse> CheckUserAsync(ClaimsPrincipal user);

    Task AddReferralAsync(ReferralRegistration referral);


}

public class ReferralService : IReferralService
{
    private readonly AppDbContext _context;

    public ReferralService(AppDbContext context) => _context = context;

    public async Task AddReferralAsync(ReferralRegistration referral)
    {
        _context.ReferralRegistrations.Add(referral);
        await _context.SaveChangesAsync();
    }


    public async Task<CheckUserResponse> CheckUserAsync(ClaimsPrincipal user)
    {
        var emailClaim = user.Claims.FirstOrDefault(c =>
            c.Type == ClaimTypes.Email || c.Type == "emails" || c.Type == "email");

        if (emailClaim == null)
        {
            return new CheckUserResponse
            {
                Exists = false,
                RedirectUrl = "/authentication/login"
            };
        }

        var email = emailClaim.Value;

        var entity = await _context.ReferralRegistrations
            .FirstOrDefaultAsync(u => u.EmailAddress == email);

        if (entity == null)
        {
            return new CheckUserResponse
            {
                Exists = false
            };
        }

        return new CheckUserResponse
        {
            Exists = true,
            referralId = entity.ReferralRegistrationId
        };
    }
}

public class CheckUserResponse
{
    public bool Exists { get; set; }
    public string RedirectUrl { get; set; }
    public Guid? referralId { get; set; }
}
