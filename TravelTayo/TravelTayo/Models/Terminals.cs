using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelTayo.Models;

namespace HotelbedsAPI.Models
{
    public class Terminal
    {
        [Key]
        public string TerminalCode { get; set; }  // e.g., "BCN"

        public string Name { get; set; }          // Optional, if available
        public double? Distance { get; set; }     // Distance to hotel or destination in km

        public long? HotelId { get; set; }        // Optional: link to Hotel
        public Hotel Hotel { get; set; }

        //public long? DestinationId { get; set; }  // Optional: link to Destination
        //public Destination Destination { get; set; }
    }
}
