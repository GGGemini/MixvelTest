using MixvelTest.Models.ProviderTwo;
using MixvelTest.Services.Interfaces;
using System.Text.Json;

namespace MixvelTest.Services
{
    public class ProviderTwoService : IProviderTwoService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        private readonly Random _random = new Random();

        public ProviderTwoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<ProviderTwoSearchResponse> SearchAsync(ProviderTwoSearchRequest request)
        {
            //var response = await _httpClient.PostAsJsonAsync("http://provider-one/api/v1/search", request);
            //response.EnsureSuccessStatusCode();

            //return await response.Content.ReadFromJsonAsync<ProviderTwoSearchResponse>(_jsonOptions);

            // Имитация задержки
            await Task.Delay(_random.Next(100, 500));

            if (_random.Next(2) == 0) // 50% шанс на успех
            {
                // Имитация успешного ответа
                return new ProviderTwoSearchResponse
                {
                    Routes = [
                        new ProviderTwoRoute
                        {
                            Departure = new ProviderTwoPoint
                            {
                                Point = request.Departure,
                                Date = request.DepartureDate
                            },
                            Arrival = new ProviderTwoPoint
                            {
                                Point = request.Arrival,
                                Date = request.DepartureDate.AddHours(2) // Примерное время прибытия
                            },
                            Price = _random.Next(200, 1000), // Случайная цена
                            TimeLimit = DateTime.Now.AddDays(1) // Примерное время "жизни" маршрута
                        }
                    ]
                };
            }
            else
            {
                // Имитация ошибки при запросе
                throw new Exception("Ошибка при запросе к ProviderTwo");
            }
        }

        public async Task<bool> IsAvailableAsync()
        {
            //try
            //{
            //    var response = await _httpClient.GetAsync("http://provider-one/api/v1/ping");
            //    return response.IsSuccessStatusCode;
            //}
            //catch
            //{
            //    return false;
            //}

            // Имитация задержки
            await Task.Delay(_random.Next(100, 500));

            // Случайное определение доступности сервиса
            return _random.Next(2) == 0; // 50% шанс на доступность
        }
    }
}
