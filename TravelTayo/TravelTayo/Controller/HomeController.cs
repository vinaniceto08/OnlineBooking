using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelTayo.Data; // Adjust namespace
using TravelTayo.Models; // Adjust namespace

namespace TravelTayo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Only authenticated users
    public class ReferralController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReferralController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("CheckUser")]
        public async Task<IActionResult> CheckUser()
        {
            try
            {
                foreach (var claim in User.Claims)
                {
                    Console.WriteLine($"Claim type: {claim.Type}, value: {claim.Value}");
                }


                var emailClaim = User.Claims.FirstOrDefault(c =>
                    c.Type == ClaimTypes.Email || c.Type == "email" || c.Type == "emails");

                if (emailClaim == null)
                {
                    Console.WriteLine($"Warning: Email claim not found for user {User.Identity?.Name ?? "Unknown"}");
                    return Unauthorized(new { message = "Email claim not found" });
                }

                var email = emailClaim.Value;
                Console.WriteLine($"Info: Checking user with email {email}");

                var user = await _context.ReferralRegistrations
                    .FirstOrDefaultAsync(u => u.EmailAddress == email);

                if (user == null)
                {
                    Console.WriteLine($"Info: Referral user not found: {email}");
                    return Ok(new { Exists = false, RedirectUrl = "/referral/referralregistration" });
                }

                Console.WriteLine($"Info: Referral user found: {email}, ID={user.ReferralRegistrationId}");
                return Ok(new { Exists = true, referralId = user.ReferralRegistrationId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CheckUser API for user {User.Identity?.Name ?? "Unknown"}: {ex}");
                return StatusCode(500, new { message = "An error occurred while checking user." });
            }
        }

    }
}
