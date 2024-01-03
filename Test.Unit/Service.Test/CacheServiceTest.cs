using Microsoft.Extensions.Caching.Memory;
using Service.Services;

namespace Test.Unit.Service.Test
{
    public class CacheServiceTest
    {
        [Fact]
        public void Get_ExistingKey_ReturnsValue()
        {
            // Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var cacheService = new CacheService(memoryCache);

            var key = "testKey1";
            var value = "testValue";
            memoryCache.Set(key, value);

            // Act
            var result = cacheService.Get<string>(key);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void Get_NonExistingKey_ReturnsDefault()
        {
            // Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var cacheService = new CacheService(memoryCache);

            var key = "nonExistingKey1";

            // Act
            var result = cacheService.Get<string>(key);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Set_CacheEntryAddedSuccessfully()
        {
            // Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var cacheService = new CacheService(memoryCache);

            var key = "testKey2";
            var value = "testValue";

            // Act
            cacheService.Set(key, value);

            // Assert
            var valueExists = memoryCache.TryGetValue(key, out string? result);
            Assert.True(valueExists);
            Assert.NotNull(result);
            Assert.Equal(value, result);
        }

        [Fact]
        public void Remove_ExistingKey_CacheEntryRemoved()
        {
            // Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var cacheService = new CacheService(memoryCache);

            var key = "testKey3";
            var value = "testValue";
            memoryCache.Set(key, value);

            // Act
            cacheService.Remove(key);

            // Assert
            Assert.Null(memoryCache.Get(key));
        }

        [Fact]
        public void Remove_NonExistingKey_NoException()
        {
            // Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var cacheService = new CacheService(memoryCache);

            var key = "nonExistingKey2";

            // Act & Assert
            try
            {
                cacheService.Remove(key);
                Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
