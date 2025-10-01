using System;
using System.ComponentModel.DataAnnotations;

public class ReferralRegistration
{
    [Key]
    public Guid ReferralRegistrationId { get; set; } = Guid.NewGuid(); // Auto-generated GUID

    [EmailAddress]
    public string EmailAddress { get; set; } // Unique reference for B2C user

    [Required(ErrorMessage = "Full Name is required")]
    [MaxLength(200)]
    public string FullName { get; set; }

    [Required]
    [MaxLength(20)]
    public string GCashNumber { get; set; }

    public string GCashQRCode { get; set; } // URL or path to uploaded QR code image

    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

    [MaxLength(50)]
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
}



