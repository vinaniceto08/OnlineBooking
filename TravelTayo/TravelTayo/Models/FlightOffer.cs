using System.ComponentModel.DataAnnotations.Schema;

[NotMapped]
public class FlightSearchResponse
{
    public FlightOffer[] Data { get; set; }
}
[NotMapped]
public class FlightOffer
{
    public string Id { get; set; }
    public Itinerary[] Itineraries { get; set; }
    public Price Price { get; set; }
}
[NotMapped]
public class Itinerary
{
    public Segment[] Segments { get; set; }
    public string Duration { get; set; }
}
[NotMapped]
public class Segment
{
    public FlightEndpoint Departure { get; set; }
    public FlightEndpoint Arrival { get; set; }
    public string CarrierCode { get; set; }
    public string Number { get; set; }
}
[NotMapped]
public class FlightEndpoint
{
    public string IataCode { get; set; }
    public string At { get; set; }
}
[NotMapped]
public class Price
{
    public string Currency { get; set; }
    public string Total { get; set; }
}
