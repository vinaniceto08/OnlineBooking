using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelbedsAPI.Models
{
    public class Facility
    {
        [Key]
        public int FacilityCode { get; set; } // maps to "facilityCode"

        public int FacilityGroupCode { get; set; }
        public FacilityGroup FacilityGroup { get; set; }

        public int Order { get; set; }          // Display or priority order
        public bool IndYesOrNo { get; set; }    // Indicates yes/no
        public int Number { get; set; }         // Quantity if applicable
        public bool Voucher { get; set; }       // Indicates voucher available

        public ICollection<HotelFacility> HotelFacilities { get; set; }
        public ICollection<RoomFacility> RoomFacilities { get; set; }
    }

    public class FacilityGroup
    {
        [Key]
        public int FacilityGroupCode { get; set; } // maps to "facilityGroupCode"

        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Facility> Facilities { get; set; }
    }

    // Junction table: Hotel ↔ Facility
    public class HotelFacility
    {
        [Key]
        public int Id { get; set; }

        public long HotelId { get; set; }
        public Hotel Hotel { get; set; }

        public int FacilityCode { get; set; }
        public Facility Facility { get; set; }
    }
}
