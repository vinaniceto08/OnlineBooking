using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelbedsAPI.Models
{
    public class Rooms
    {
        [Key]
        public string RoomCode { get; set; } // maps to "roomCode"

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }

        public bool IsParentRoom { get; set; }
        public int MinPax { get; set; }
        public int MaxPax { get; set; }
        public int MaxAdults { get; set; }
        public int MaxChildren { get; set; }
        public int MinAdults { get; set; }

        public string RoomType { get; set; }
        public string CharacteristicCode { get; set; }

        public List<RoomFacility> RoomFacilities { get; set; }
        public List<RoomStay> RoomStays { get; set; }
    }

    public class RoomFacility
    {
        [Key]
        public int Id { get; set; }
        public Rooms Room { get; set; }

        public int FacilityCode { get; set; }
        public int FacilityGroupCode { get; set; }
        public int Number { get; set; }
        public bool IndYesOrNo { get; set; }
        public bool Voucher { get; set; }
    }

    public class RoomStay
    {
        [Key]
        public int Id { get; set; }
        public Rooms Rooms { get; set; }

        public string StayType { get; set; } // e.g., "BED"
        public int Order { get; set; } // sequence/order
        public string Description { get; set; }

   
    }



    public class RoomType
    {
        [Key]
        public int Id { get; set; }
        public string? Code { get; set; }

        public Rooms Rooms { get; set; }

    }
}
