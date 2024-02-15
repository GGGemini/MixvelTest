using Xunit;
using Moq;
using MixvelTest.Services;
using MixvelTest.Services.Interfaces;
using MixvelTest.Agregators;
using MixvelTest.Mediators;
using MixvelTest.Models.ProviderOne;
using MixvelTest.Models.ProviderTwo;
using MixvelTest.Models;

namespace MixvelTest.Tests
{
    public class SearchServiceTests
    {
        private readonly Mock<IProviderOneService> _mockProviderOneService = new Mock<IProviderOneService>();
        private readonly Mock<IProviderTwoService> _mockProviderTwoService = new Mock<IProviderTwoService>();
        private readonly Mock<ISearchResponseAggregator> _mockAggregator = new Mock<ISearchResponseAggregator>();
        private readonly Mock<ISearchRequestMediator> _mockMediator = new Mock<ISearchRequestMediator>();
        private readonly Mock<ICacheService> _mockCacheService = new Mock<ICacheService>();

        private readonly SearchService _searchService;

        public SearchServiceTests()
        {
            _searchService = new SearchService(
                _mockProviderOneService.Object,
                _mockProviderTwoService.Object,
                _mockAggregator.Object,
                _mockMediator.Object,
                _mockCacheService.Object);
        }

        [Fact]
        public async Task SearchAsync_AggregatesAndCachesData_WhenOnlyCachedIsFalse()
        {
            // Arrange
            var mockRequest = new SearchRequest
            {
                Origin = "Origin",
                Destination = "Destination",
                OriginDateTime = DateTime.Now,
                Filters = new SearchFilters { OnlyCached = false }
            };
            var providerOneResponse = new ProviderOneSearchResponse { /* Данные от ProviderOne */ };
            var providerTwoResponse = new ProviderTwoSearchResponse { /* Данные от ProviderTwo */ };
            var expectedResponse = new SearchResponse { /* Ожидаемая агрегация данных */ };

            _mockProviderOneService.Setup(x => x.SearchAsync(It.IsAny<ProviderOneSearchRequest>()))
                                   .ReturnsAsync(providerOneResponse);
            _mockProviderTwoService.Setup(x => x.SearchAsync(It.IsAny<ProviderTwoSearchRequest>()))
                                   .ReturnsAsync(providerTwoResponse);
            _mockAggregator.Setup(x => x.AggregateResponses(providerOneResponse, providerTwoResponse))
                           .Returns(expectedResponse);

            // Act
            var result = await _searchService.SearchAsync(mockRequest);

            // Assert
            Assert.Equal(expectedResponse, result);
            _mockCacheService.Verify(x => x.SetCachedDataAsync(
                It.IsAny<string>(),
                expectedResponse,
                It.IsAny<TimeSpan?>(),
                It.IsAny<TimeSpan?>()),
                Times.Once);
        }

        [Fact]
        public async Task SearchAsync_ReturnsDataFromCache_WhenOnlyCachedIsTrueAndCacheIsNotEmpty()
        {
            // Arrange
            var mockRequest = new SearchRequest
            {
                Origin = "Origin",
                Destination = "Destination",
                OriginDateTime = DateTime.Now,
                Filters = new SearchFilters { OnlyCached = true }
            };
            var expectedResponse = new SearchResponse { /* Ожидаемый ответ */ };
            var cacheKey = $"search_{mockRequest.Origin}_{mockRequest.Destination}_{mockRequest.OriginDateTime.Ticks}";

            _mockCacheService.Setup(x => x.GetCachedDataAsync<SearchResponse>(It.IsAny<string>()))
                             .ReturnsAsync(expectedResponse);

            // Act
            var result = await _searchService.SearchAsync(mockRequest);

            // Assert
            Assert.Equal(expectedResponse, result);
            _mockProviderOneService.Verify(x => x.SearchAsync(It.IsAny<ProviderOneSearchRequest>()), Times.Never);
            _mockProviderTwoService.Verify(x => x.SearchAsync(It.IsAny<ProviderTwoSearchRequest>()), Times.Never);
        }

        [Fact]
        public async Task SearchAsync_ThrowsException_WhenOnlyCachedIsTrueAndCacheIsEmpty()
        {
            // Arrange
            var mockRequest = new SearchRequest
            {
                Origin = "Origin",
                Destination = "Destination",
                OriginDateTime = DateTime.Now,
                Filters = new SearchFilters { OnlyCached = true }
            };
            var cacheKey = $"search_{mockRequest.Origin}_{mockRequest.Destination}_{mockRequest.OriginDateTime.Ticks}";

            _mockCacheService.Setup(x => x.GetCachedDataAsync<SearchResponse>(It.IsAny<string>()))
                             .ReturnsAsync((SearchResponse)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _searchService.SearchAsync(mockRequest));
        }

        [Fact]
        public async Task IsAvailableAsync_ReturnsTrue_WhenBothServicesAreAvailable()
        {
            // Arrange
            _mockProviderOneService.Setup(service => service.IsAvailableAsync()).ReturnsAsync(true);
            _mockProviderTwoService.Setup(service => service.IsAvailableAsync()).ReturnsAsync(true);

            // Act
            var result = await _searchService.IsAvailableAsync();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsAvailableAsync_ReturnsFalse_WhenAnyServiceIsUnavailable()
        {
            // Arrange
            _mockProviderOneService.Setup(service => service.IsAvailableAsync()).ReturnsAsync(false); // Первый сервис недоступен
            _mockProviderTwoService.Setup(service => service.IsAvailableAsync()).ReturnsAsync(true); // Второй сервис доступен

            // Act
            var result = await _searchService.IsAvailableAsync();

            // Assert
            Assert.False(result);
        }
    }
}
