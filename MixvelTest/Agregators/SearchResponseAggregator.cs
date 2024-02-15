using MixvelTest.Models.ProviderOne;
using MixvelTest.Models.ProviderTwo;
using MixvelTest.Models;
using Route = MixvelTest.Models.Route;
using MixvelTest.Mediators;

namespace MixvelTest.Agregators
{
    public class SearchResponseAggregator : ISearchResponseAggregator
    {

        private readonly ISearchRequestMediator _mediator;

        public SearchResponseAggregator(ISearchRequestMediator mediator)
        {
            _mediator = mediator;
        }

        public SearchResponse AggregateResponses(ProviderOneSearchResponse providerOneResponse, ProviderTwoSearchResponse providerTwoResponse)
        {
            var routes = new List<Route>();

            // Маппинг маршрутов от ProviderOne
            if (providerOneResponse.Routes != null)
            {
                routes.AddRange(providerOneResponse.Routes.Select(_mediator.MapFromProviderOneResponseRoute));
            }

            // Маппинг маршрутов от ProviderTwo
            if (providerTwoResponse.Routes != null)
            {
                routes.AddRange(providerTwoResponse.Routes.Select(_mediator.MapFromProviderTwoResponseRoute));
            }

            // Предполагаем, что SearchResponse требует также определения минимальной и максимальной цены,
            // а также самого быстрого и самого медленного маршрута. Эти значения можно вычислить на основе
            // списка routes.
            var minPrice = routes.Min(r => r.Price);
            var maxPrice = routes.Max(r => r.Price);

            var orderedRoutes = routes.OrderBy(r => r.DestinationDateTime - r.OriginDateTime);
            var fastestRoute = orderedRoutes.First();
            var slowestRoute = orderedRoutes.Last();

            var response = new SearchResponse
            {
                Routes = routes.ToArray(),
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                MinMinutesRoute = (int)(fastestRoute.DestinationDateTime - fastestRoute.OriginDateTime).TotalMinutes,
                MaxMinutesRoute = (int)(slowestRoute.DestinationDateTime - slowestRoute.OriginDateTime).TotalMinutes,
            };

            return response;
        }
    }
}
