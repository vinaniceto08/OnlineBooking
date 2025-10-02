using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TravelTayo.Data;
using TravelTayo.Models;

namespace TravelTayoUnitTest
{
    public static class TestDbContext
    {
        public static AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }
    }

    [TestFixture]
    public class AccountB2CTests
    {
        private AppDbContext _dbContext;

        [SetUp]
        public void Setup()
        {
            _dbContext = TestDbContext.GetInMemoryDbContext();
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }

        // Simulate B2C login
        private async Task<Account> SimulateB2CLogin(string b2cUserId, string email, string fullName)
        {
            // Check if account already exists
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.B2CUserId == b2cUserId);
            if (account != null)
            {
                return account;
            }

            // Create new account if doesn't exist
            account = new Account
            {
                B2CUserId = b2cUserId,
                Email = email,
                FullName = fullName
            };

            _dbContext.Accounts.Add(account);
            await _dbContext.SaveChangesAsync();
            return account;
        }

        // Check if contact number is filled
        private bool NeedsContactNumber(Account account)
        {
            return string.IsNullOrEmpty(account.ContactNumber);
        }

        [Test]
        public async Task NewB2CUser_RedirectToAccountInfoIfNoContactNumber()
        {
            // Simulate new B2C login
            var account = await SimulateB2CLogin("b2c-user-100", "newuser@example.com", "New User");

            // Assert account is created
            Assert.IsNotNull(account);
            Assert.AreEqual("New User", account.FullName);
            Assert.IsNull(account.ContactNumber);

            // Check if redirect needed
            Assert.IsTrue(NeedsContactNumber(account));
        }

        [Test]
        public async Task ExistingB2CUserWithContactNumber_NoRedirect()
        {
            // Arrange: create existing account with contact number
            var existing = new Account
            {
                B2CUserId = "b2c-user-101",
                Email = "existing@example.com",
                FullName = "Existing User",
                ContactNumber = "09171234567"
            };
            _dbContext.Accounts.Add(existing);
            await _dbContext.SaveChangesAsync();

            // Simulate B2C login
            var account = await SimulateB2CLogin("b2c-user-101", "existing@example.com", "Existing User");

            // Assert account exists and contact number is present
            Assert.IsNotNull(account);
            Assert.AreEqual("09171234567", account.ContactNumber);

            // Check if redirect needed
            Assert.IsFalse(NeedsContactNumber(account));
        }

        [Test]
        public async Task UpdateContactNumberAfterRedirect_ShouldSave()
        {
            // Simulate new B2C login
            var account = await SimulateB2CLogin("b2c-user-102", "user102@example.com", "User 102");

            // User fills contact number
            account.ContactNumber = "09991234567";
            await _dbContext.SaveChangesAsync();

            // Reload and assert
            var updated = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.B2CUserId == "b2c-user-102");
            Assert.AreEqual("09991234567", updated.ContactNumber);
            Assert.IsFalse(NeedsContactNumber(updated));
        }
    }
}
