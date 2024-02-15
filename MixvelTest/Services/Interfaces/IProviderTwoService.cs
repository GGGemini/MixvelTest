using MixvelTest.Models.ProviderTwo;

namespace MixvelTest.Services.Interfaces
{
    public interface IProviderTwoService
    {
        Task<ProviderTwoSearchResponse> SearchAsync(ProviderTwoSearchRequest request);
        Task<bool> IsAvailableAsync();
    }
}
