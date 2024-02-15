using MixvelTest.Models.ProviderOne;
using MixvelTest.Services.Interfaces;
using System;
using System.Text.Json;

namespace MixvelTest.Services
{
    public class ProviderOneService : IProviderOneService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        private readonly Random _random = new Random();

        public ProviderOneService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<ProviderOneSearchResponse> SearchAsync(ProviderOneSearchRequest request)
        {
            //var response = await _httpClient.PostAsJsonAsync("http://provider-one/api/v1/search", request);
            //response.EnsureSuccessStatusCode();

            //return await response.Content.ReadFromJsonAsync<ProviderOneSearchResponse>(_jsonOptions);

            // Имитация задержки
            await Task.Delay(_random.Next(100, 500));

            // Случайное определение успешности запроса
            if (_random.Next(2) == 0) // 50% шанс
            {
                // Имитация успешного ответа
                return new ProviderOneSearchResponse
                {
                    Routes = [
                        new ProviderOneRoute
                        {
                            From = request.From,
                            To = request.To,
                            DateFrom = request.DateFrom,
                            DateTo = request.DateTo ?? request.DateFrom.AddHours(2),
                            Price = _random.Next(100, 500),
                            TimeLimit = DateTime.Now.AddDays(1)
                        }
                    ]
                };
            }
            else
            {
                // Имитация ошибки при запросе
                throw new Exception("Ошибка при запросе к ProviderOne");
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
            return _random.Next(2) == 0; // 50% шанс
        }
    }
}
