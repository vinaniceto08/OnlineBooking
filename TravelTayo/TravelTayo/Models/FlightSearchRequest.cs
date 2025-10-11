public class FlightSearchRequest
{
    public bool IsRoundTrip { get; set; } = false; // false = One Way, true = Round Trip

    public string Origin { get; set; }
    public string Destination { get; set; }

    public string DepartureDate { get; set; }
    public string? ReturnDate { get; set; } // Only used if IsRoundTrip = true

    public int Adults { get; set; } = 1;
    public int Children { get; set; } = 0;
    public int Infants { get; set; } = 0;

    public string PlaneTypeInput { get; set; }
}
