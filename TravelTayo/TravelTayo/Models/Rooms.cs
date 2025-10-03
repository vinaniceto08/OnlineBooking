using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelbedsAPI.Models
{
    public class Room
    {
        [Key]
        public string RoomCode { get; set; } // maps to "roomCode"

        public long HotelId { get; set; }
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

        public string RoomCode { get; set; }
        public Room Room { get; set; }

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

        public string RoomCode { get; set; }
        public Room Room { get; set; }

        public string StayType { get; set; } // e.g., "BED"
        public int Order { get; set; } // sequence/order
        public string Description { get; set; }

        public List<RoomStayFacility> RoomStayFacilities { get; set; }
    }

    public class RoomStayFacility
    {
        [Key]
        public int Id { get; set; }

        public int RoomStayId { get; set; }
        public RoomStay RoomStay { get; set; }

        public string PMSRoomCode { get; set; } // e.g., "N/A"
    }
}
