using MixvelTest.Models.ProviderOne;

namespace MixvelTest.Services.Interfaces
{
    public interface IProviderOneService
    {
        Task<ProviderOneSearchResponse> SearchAsync(ProviderOneSearchRequest request);
        Task<bool> IsAvailableAsync();
    }
}
