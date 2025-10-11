using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

public class AmadeusOptions
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}

public class AmadeusService : IAmadeusService
{
    private readonly HttpClient _http;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private string _accessToken;
    private DateTime _tokenExpiry;

    private const string TokenUrl = "https://test.api.amadeus.com/v1/security/oauth2/token";
    private const string FlightSearchUrl = "https://test.api.amadeus.com/v2/shopping/flight-offers";

    public AmadeusService(HttpClient httpClient, IOptions<AmadeusOptions> amadeusOptions)
    {
        _http = httpClient;
        _clientId = amadeusOptions.Value.ClientId;
        _clientSecret = amadeusOptions.Value.ClientSecret;
    }

    private async Task EnsureTokenAsync()
    {
        if (!string.IsNullOrEmpty(_accessToken) && DateTime.UtcNow < _tokenExpiry.AddSeconds(-60))
            return;

        var body = new StringContent(
            $"grant_type=client_credentials&client_id={Uri.EscapeDataString(_clientId)}&client_secret={Uri.EscapeDataString(_clientSecret)}",
            Encoding.UTF8, "application/x-www-form-urlencoded");

        using var res = await _http.PostAsync(TokenUrl, body);
        res.EnsureSuccessStatusCode();
        using var stream = await res.Content.ReadAsStreamAsync();
        using var doc = await JsonDocument.ParseAsync(stream);

        _accessToken = doc.RootElement.GetProperty("access_token").GetString();
        int expiresIn = doc.RootElement.GetProperty("expires_in").GetInt32();
        _tokenExpiry = DateTime.UtcNow.AddSeconds(expiresIn);
    }

    public async Task<FlightOffer[]> SearchFlightsAsync(string origin, string destination, string departureDate, string? returnDate, int adults, int children, int infants, string cabinClass)
    {
        await EnsureTokenAsync();

        if (string.IsNullOrEmpty(origin) || string.IsNullOrEmpty(destination) || string.IsNullOrEmpty(departureDate))
            throw new ArgumentException("Origin, destination, and departure date are required.");

        cabinClass = string.IsNullOrEmpty(cabinClass) ? "ECONOMY" : cabinClass.ToUpper();
        origin = ExtractCode(origin);
        destination = ExtractCode(destination);

        var uri = $"{FlightSearchUrl}?originLocationCode={origin}" +
                  $"&destinationLocationCode={destination}" +
                  $"&departureDate={departureDate}" +
                  $"&adults={adults}" +
                  (children > 0 ? $"&children={children}" : "") +
                  (infants > 0 ? $"&infants={infants}" : "") +
                  $"&travelClass={cabinClass}" +
                  (!string.IsNullOrEmpty(returnDate) ? $"&returnDate={returnDate}" : "") +
                  $"&max=10";

        using var req = new HttpRequestMessage(HttpMethod.Get, uri);
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

        using var res = await _http.SendAsync(req);
        var content = await res.Content.ReadAsStringAsync();

        // ✅ Deserialize instead of returning string
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var result = JsonSerializer.Deserialize<FlightSearchResponse>(content, options);

        if (!res.IsSuccessStatusCode)
            throw new Exception($"Amadeus API error: {res.StatusCode} - {content}");

        return result?.Data ?? Array.Empty<FlightOffer>();
    }

    string ExtractCode(string value)
    {
        if (string.IsNullOrEmpty(value)) return value;
        var match = System.Text.RegularExpressions.Regex.Match(value, @"\(([A-Z]{3})\)");
        return match.Success ? match.Groups[1].Value : value;
    }



}
