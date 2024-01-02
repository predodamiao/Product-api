namespace Service.Services.Interfaces
{
    /// <summary>
    /// Cache Service
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Get a value from the cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T? Get<T>(string key);

        /// <summary>
        /// Set a value in the cache with a expiration time
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expirationInMinutes"></param>
        void Set<T>(string key, T value, int expirationInMinutes = 100);

        /// <summary>
        /// Remove a value from the cache
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
    }
}
