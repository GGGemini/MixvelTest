using MixvelTest.Models.ProviderOne;
using MixvelTest.Models.ProviderTwo;
using MixvelTest.Models;
using Route = MixvelTest.Models.Route;

namespace MixvelTest.Mediators
{
    public class SearchRequestMediator : ISearchRequestMediator
    {
        public ProviderOneSearchRequest MapToProviderOneSearchRequest(SearchRequest request)
        {
            // Маппинг для ProviderOne может выглядеть примерно так
            var providerOneRequest = new ProviderOneSearchRequest
            {
                From = request.Origin,
                To = request.Destination,
                DateFrom = request.OriginDateTime,
                // Маппинг DateTo зависит от того, предполагает ли поставщик ProviderOne диапазон дат или конкретную дату
                DateTo = request.Filters?.DestinationDateTime,
                MaxPrice = request.Filters?.MaxPrice
            };

            return providerOneRequest;
        }

        public ProviderTwoSearchRequest MapToProviderTwoSearchRequest(SearchRequest request)
        {
            // Маппинг для ProviderTwo может быть другим, в зависимости от ожиданий API ProviderTwo
            var providerTwoRequest = new ProviderTwoSearchRequest
            {
                Departure = request.Origin,
                Arrival = request.Destination,
                DepartureDate = request.OriginDateTime,
                // ProviderTwo может ожидать другую информацию, например, минимальное время жизни маршрута
                MinTimeLimit = request.Filters?.MinTimeLimit
            };

            return providerTwoRequest;
        }

        public Route MapFromProviderOneResponseRoute(ProviderOneRoute r)
        {
            var route = new Route
            {
                Id = Guid.NewGuid(),
                Origin = r.From,
                Destination = r.To,
                OriginDateTime = r.DateFrom,
                DestinationDateTime = r.DateTo,
                Price = r.Price,
                TimeLimit = r.TimeLimit
            };

            return route;
        }

        public Route MapFromProviderTwoResponseRoute(ProviderTwoRoute r)
        {
            var route = new Route
            {
                Id = Guid.NewGuid(),
                Origin = r.Departure.Point,
                Destination = r.Arrival.Point,
                OriginDateTime = r.Departure.Date,
                DestinationDateTime = r.Arrival.Date,
                Price = r.Price,
                TimeLimit = r.TimeLimit
            };

            return route;
        }
    }
}
