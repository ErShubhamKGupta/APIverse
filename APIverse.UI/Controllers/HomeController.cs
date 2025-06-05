using APIverse.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace APIverse.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index() => View();

        public IActionResult DigiPin() => View();

        [HttpPost]
        public async Task<IActionResult> GenerateDigiPin(double latitude, double longitude)
        {
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(
                JsonSerializer.Serialize(new { latitude, longitude }),
                Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7028/api/digipin", content);

            if (!response.IsSuccessStatusCode)
            {
                TempData["DigiPinResult"] = "API Error";
            }
            else
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON to extract just the DIGIPIN value
                var result = JsonSerializer.Deserialize<DigiPinResponse>(jsonResponse);
                TempData["DigiPinResult"] = result?.digiPin ?? "No DIGIPIN returned";
            }

            return RedirectToAction("DigiPin");
        }

        public class DigiPinResponse
        {
            public string digiPin { get; set; }
        }
    }

}
