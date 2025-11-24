using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ShopAPI.Api.Client
{
    public class CityDto
    {
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }

    public class CityClient
    {
        private readonly HttpClient _http;
        public CityClient(HttpClient http) => _http = http;

        public async Task<List<CityDto>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<CityDto>>("/api/cities") ?? new List<CityDto>();
        }

        public async Task<CityDto?> GetAsync(int id)
        {
            return await _http.GetFromJsonAsync<CityDto>($"/api/cities/{id}");
        }

        public async Task<HttpResponseMessage> CreateAsync(CityDto dto)
        {
            var resp = await _http.PostAsJsonAsync("/api/cities", dto);
            return resp;
        }

        public async Task<HttpResponseMessage> UpdateAsync(int id, CityDto dto)
        {
            var resp = await _http.PutAsJsonAsync($"/api/cities/{id}", dto);
            return resp;
        }

        public async Task<HttpResponseMessage> PatchCountryAsync(int id, string country)
        {
            var payload = JsonSerializer.Serialize(new { Country = country });
            var content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
            var req = new HttpRequestMessage(new HttpMethod("PATCH"), $"/api/cities/{id}") { Content = content };
            return await _http.SendAsync(req);
        }

        public async Task<HttpResponseMessage> DeleteAsync(int id)
        {
            return await _http.DeleteAsync($"/api/cities/{id}");
        }
    }
}
