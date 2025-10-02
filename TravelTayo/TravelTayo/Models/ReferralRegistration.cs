using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TravelTayo.Models
{

    public class ReferralRegistration
    {
        [Key]
        public Guid ReferralRegistrationId { get; set; } = Guid.NewGuid(); // Auto-generated GUID

        [EmailAddress]
        public string EmailAddress { get; set; } // Unique reference for B2C user

       
        [MaxLength(200)]
        public string FullName { get; set; }


        public string GCashQRCode { get; set; } // URL or path to uploaded QR code image

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected


        [MaxLength(200)]
        public string ReferralLink { get; set; }     }



}

