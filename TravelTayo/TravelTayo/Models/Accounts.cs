using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TravelTayo.Models
{
    [Table("Accounts")]
    [Index(nameof(B2CUserId), IsUnique = true)] // Ensure one account per B2C user
    public class Account
    {
        [Key]
        public Guid AccountId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string B2CUserId { get; set; }  // Unique ID from Azure B2C

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string FullName { get; set; }

        [MaxLength(20)]
        public string? ContactNumber { get; set; }  // Nullable until user fills it

        [Column(TypeName = "decimal(18,2)")]
        public decimal WalletBalance { get; set; } = 0m;

        public DateTime DateCreated { get; set; } = DateTime.UtcNow; // Better for API consistency

        public DateTime? LastLogin { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Active";

        [MaxLength(50)]
        public string? ReferralCode { get; set; }
    }
}
