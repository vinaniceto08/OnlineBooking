using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravelTayo.Models
{
    public class Hotel
    {
        [Key]
        public int HotelId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
