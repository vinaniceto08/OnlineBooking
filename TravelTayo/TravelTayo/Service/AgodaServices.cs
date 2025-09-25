using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TravelTayo.Data;
using TravelTayo.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TravelTayo.Services
{
    public class AgodaService
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _dbContext;
        private readonly string _apiKey; // from Agoda Affiliate

        public AgodaService(HttpClient httpClient, AppDbContext dbContext, string apiKey)
        {
            _httpClient = httpClient;
            _dbContext = dbContext;
            _apiKey = apiKey;
        }

        // Step 1: City Search
        public async Task<dynamic> CitySearchAsync(CitySearchCriteria criteria)
        {
            var requestJson = JsonConvert.SerializeObject(new { criteria });
            var request = new HttpRequestMessage(HttpMethod.Post, "https://affiliate-api.agoda.com/city-search")
            {
                Content = new StringContent(requestJson, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("ApiKey", _apiKey);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(json);

            // Optional: save hotel IDs in DB
            foreach (var hotel in result.hotels)
            {
                var dbHotel = await _dbContext.Hotels.FindAsync((int)hotel.id) ?? new Hotel { HotelId = hotel.id };
                dbHotel.Name = hotel.name;
                dbHotel.City = hotel.city;
                dbHotel.ImageUrl = hotel.imageUrl;
                dbHotel.LastUpdated = DateTime.UtcNow;

                _dbContext.Hotels.Update(dbHotel);
            }
            await _dbContext.SaveChangesAsync();

            return result;
        }

        // Step 2: Hotel List Search (live price & availability)
        public async Task<dynamic> HotelListSearchAsync(HotelListSearchCriteria criteria)
        {
            var requestJson = JsonConvert.SerializeObject(new { criteria });
            var request = new HttpRequestMessage(HttpMethod.Post, "https://affiliate-api.agoda.com/hotel-list-search")
            {
                Content = new StringContent(requestJson, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("ApiKey", _apiKey);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(json);

            // Optional: update DB with price/availability
            foreach (var hotel in result.hotels)
            {
                var dbHotel = await _dbContext.Hotels.FindAsync((int)hotel.id);
                if (dbHotel != null)
                {
                    dbHotel.Price = hotel.price;
                    dbHotel.Currency = hotel.currency;
                    dbHotel.IsAvailable = hotel.available;
                    dbHotel.LastUpdated = DateTime.UtcNow;
                    _dbContext.Hotels.Update(dbHotel);
                }
            }
            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
