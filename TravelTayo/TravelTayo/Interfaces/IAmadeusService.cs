using System.Threading.Tasks;

public interface IAmadeusService
{
    Task<FlightOffer[]> SearchFlightsAsync(
       string origin,
       string destination,
       string departureDate,
       string? returnDate,
       int adults = 1,
       int children = 0,
       int infants = 0,
       string cabinClass = "ECONOMY"
   );
}
