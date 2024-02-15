using MixvelTest.Models;

namespace MixvelTest.Services.Interfaces
{
    public interface ISearchService
    {
        Task<SearchResponse> SearchAsync(SearchRequest request);
        Task<bool> IsAvailableAsync();
    }
}
