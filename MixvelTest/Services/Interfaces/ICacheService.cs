namespace MixvelTest.Services.Interfaces
{
    public interface ICacheService
    {
        Task<T> GetCachedDataAsync<T>(string cacheKey);
        Task SetCachedDataAsync<T>(string cacheKey, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null);
    }
}
