using MixvelTest.Agregators;
using MixvelTest.Mediators;
using MixvelTest.Models;
using MixvelTest.Services.Interfaces;

namespace MixvelTest.Services
{
    public class SearchService : ISearchService
    {
        private readonly IProviderOneService _providerOneService;
        private readonly IProviderTwoService _providerTwoService;
        private readonly ISearchResponseAggregator _aggregator;
        private readonly ISearchRequestMediator _mediator;
        private readonly ICacheService _cacheService;

        public SearchService(
            IProviderOneService providerOneService,
            IProviderTwoService providerTwoService,
            ISearchResponseAggregator aggregator,
            ISearchRequestMediator mediator,
            ICacheService cacheService)
        {
            _providerOneService = providerOneService;
            _providerTwoService = providerTwoService;
            _aggregator = aggregator;
            _mediator = mediator;
            _cacheService = cacheService;
        }

        public async Task<SearchResponse> SearchAsync(SearchRequest request)
        {
            var cacheKey = $"search_{request.Origin}_{request.Destination}_{request.OriginDateTime.Ticks}";

            if (request.Filters?.OnlyCached ?? false)
            {
                // Попытка извлечения данных из кэша                
                var cachedResponse = await _cacheService.GetCachedDataAsync<SearchResponse>(cacheKey);
                if (cachedResponse != null)
                {
                    return cachedResponse;
                }
                else
                {
                    // Обработка случая, когда в кэше нет данных, но запросился OnlyCached
                    throw new Exception("Cache is Empty");
                }
            }
            else
            {
                // Преобразование запроса для каждого из провайдеров
                var providerOneRequest = _mediator.MapToProviderOneSearchRequest(request);
                var providerTwoRequest = _mediator.MapToProviderTwoSearchRequest(request);

                // Выполнение асинхронных запросов к каждому из провайдеров
                var providerOneResponseTask = _providerOneService.SearchAsync(providerOneRequest);
                var providerTwoResponseTask = _providerTwoService.SearchAsync(providerTwoRequest);

                await Task.WhenAll(providerOneResponseTask, providerTwoResponseTask);

                // Получение результатов
                var providerOneResponse = await providerOneResponseTask;
                var providerTwoResponse = await providerTwoResponseTask;

                // Агрегация ответов от провайдеров в единый SearchResponse
                var searchResponse = _aggregator.AggregateResponses(providerOneResponse, providerTwoResponse);

                // Установка значения в кэш
                await _cacheService.SetCachedDataAsync(cacheKey, searchResponse, TimeSpan.FromMinutes(10)); // Пример времени жизни кэша

                return searchResponse;
            }
        }

        public async Task<bool> IsAvailableAsync()
        {
            // Проверка доступности наших сервисов
            var providerOneAvailable = await _providerOneService.IsAvailableAsync();
            var providerTwoAvailable = await _providerTwoService.IsAvailableAsync();

            return providerOneAvailable && providerTwoAvailable;
        }
    }
}
