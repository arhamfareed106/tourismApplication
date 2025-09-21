using System.Text.Json;
using tourismApplication.Models;


namespace tourismApplication.Services
{
    public class TourJsonService
    {
        private readonly string _filePath;

        public TourJsonService(IWebHostEnvironment env)
        {
            // File path: wwwroot/data/tours.json
            _filePath = Path.Combine(env.ContentRootPath, "Data", "tours.json");
        }

        public IEnumerable<Tour> GetTours()
        {
            if (!File.Exists(_filePath))
                return new List<Tour>();

            var json = File.ReadAllText(_filePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<Tour>>(json, options) ?? new List<Tour>();
        }
    }
}
