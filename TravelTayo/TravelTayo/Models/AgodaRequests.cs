namespace TravelTayo.Models
{
    public class Occupancy
    {
        public int NumberOfAdult { get; set; }
        public int NumberOfChildren { get; set; }
    }

    public class Additional
    {
        public string Currency { get; set; }
        public bool DiscountOnly { get; set; }
        public string Language { get; set; }
        public Occupancy Occupancy { get; set; }
        public int MaxResult { get; set; }
        public int MinimumReviewScore { get; set; }
        public int MinimumStarRating { get; set; }
        public SortByEnum SortBy { get; set; }
        public DailyRate DailyRate { get; set; }
    }

    public class DailyRate
    {
        public decimal Minimum { get; set; }
        public decimal Maximum { get; set; }
    }

    public enum SortByEnum
    {
        PriceAsc,
        PriceDesc,
        RatingDesc
    }

    public class CitySearchCriteria
    {
        public Additional Additional { get; set; }
        public string CheckInDate { get; set; }
        public string CheckOutDate { get; set; }
        public int CityId { get; set; }
    }

    public class HotelListSearchCriteria
    {
        public Additional Additional { get; set; }
        public string CheckInDate { get; set; }
        public string CheckOutDate { get; set; }
        public int[] HotelId { get; set; }
    }
}
