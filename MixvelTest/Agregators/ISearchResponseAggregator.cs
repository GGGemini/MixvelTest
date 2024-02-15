using MixvelTest.Models.ProviderOne;
using MixvelTest.Models.ProviderTwo;
using MixvelTest.Models;

namespace MixvelTest.Agregators
{
    public interface ISearchResponseAggregator
    {
        SearchResponse AggregateResponses(ProviderOneSearchResponse providerOneResponse, ProviderTwoSearchResponse providerTwoResponse);
    }
}
