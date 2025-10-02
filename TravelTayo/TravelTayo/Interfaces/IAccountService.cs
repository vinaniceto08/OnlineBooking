using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using TravelTayo.Data;
using TravelTayo.Models;



namespace TravelTayo.Interfaces
{
    public interface IAccountService
    {
        Task<Account> GetByB2CUserIdAsync(string b2cuserid);
        Task AddOrUpdateAsync(Account account);


    }

    public class AccountService : IAccountService {

        private readonly AppDbContext _context;
        public AccountService(AppDbContext context) { 
            
            _context = context;
        }

        public async Task AddOrUpdateAsync(Account account)
        {
            if (string.IsNullOrEmpty(account.B2CUserId))
                throw new ArgumentException("B2CUserId is required.", nameof(account.B2CUserId));

            var existing = await _context.Accounts
                .FirstOrDefaultAsync(a => a.B2CUserId == account.B2CUserId);

            if (existing != null)
            {
                if (!string.IsNullOrWhiteSpace(account.FullName))
                    existing.FullName = account.FullName;

                if (!string.IsNullOrWhiteSpace(account.Email))
                    existing.Email = account.Email;

                if (!string.IsNullOrWhiteSpace(account.ContactNumber))
                    existing.ContactNumber = account.ContactNumber;

                existing.LastLogin = DateTime.UtcNow;
                _context.Accounts.Update(existing);
            }
            else
            {
                account.DateCreated = DateTime.UtcNow;
                _context.Accounts.Add(account);
            }

            await _context.SaveChangesAsync();
        }


        public async Task<Account> GetByB2CUserIdAsync(string b2cuserid)
        {
            return await _context.Accounts.FirstOrDefaultAsync(c => c.B2CUserId == b2cuserid);
        }

    }
}
