using MixvelTest.Models.ProviderOne;
using MixvelTest.Models.ProviderTwo;
using MixvelTest.Models;
using Route = MixvelTest.Models.Route;

namespace MixvelTest.Mediators
{
    public interface ISearchRequestMediator
    {
        ProviderOneSearchRequest MapToProviderOneSearchRequest(SearchRequest request);
        ProviderTwoSearchRequest MapToProviderTwoSearchRequest(SearchRequest request);
        Route MapFromProviderOneResponseRoute(ProviderOneRoute r);
        Route MapFromProviderTwoResponseRoute(ProviderTwoRoute r);
    }
}
