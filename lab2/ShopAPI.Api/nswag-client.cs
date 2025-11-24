// Auto-generated simplified client (approximation of NSwag output)
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopApi.Client
{
    public class CityDto
    {
        public string? Name { get; set; }
        public string? Country { get; set; }
    }

    public class CityCreate
    {
        public string? Name { get; set; }
        public string? Country { get; set; }
        public int? Population { get; set; }
    }

    public class CityUpdate
    {
        public string? Name { get; set; }
        public string? Country { get; set; }
        public int? Population { get; set; }
    }

    public partial class ShopApiClient
    {
        private readonly HttpClient _httpClient;
        public ShopApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CityDto>> GetCitiesAsync()
        {
            var response = await _httpClient.GetAsync("/api/cities");
            response.EnsureSuccessStatusCode();
            var s = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<CityDto>>(s, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<CityDto>();
        }

        public async Task<CityDto?> GetCityAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/cities/{id}");
            if (response.IsSuccessStatusCode)
            {
                var s = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<CityDto>(s, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return null;
        }

        public async Task<HttpResponseMessage> CreateCityAsync(CityCreate body)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/cities", body);
            return response;
        }

        public async Task<HttpResponseMessage> UpdateCityAsync(int id, CityUpdate body)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/cities/{id}", body);
            return response;
        }

        public async Task<HttpResponseMessage> PatchCityCountryAsync(int id, string country)
        {
            var payload = JsonSerializer.Serialize(new { Country = country });
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"/api/cities/{id}") { Content = new StringContent(payload, Encoding.UTF8, "application/json") };
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> DeleteCityAsync(int id)
        {
            return await _httpClient.DeleteAsync($"/api/cities/{id}");
        }
    }
}
