using System;
using System.Configuration;
using Funq;
using library.caching;

namespace triperoo.apis.Configuration
{
    /// <summary>
    /// Static class used to register every repository required by the application
    /// </summary>
    public static class Repositories
    {
        /// <summary>
        /// Register the required repositories
        /// </summary>
        public static void Register(Container container)
		{
            // The cache should live for the duration of the container
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["UseAzure"]))
            {
				container.Register<ICacheProvider>(c => new RedisCacheClient(ConfigurationManager.AppSettings["redis:connectionString"], ConfigurationManager.AppSettings["redis:namespace"], TimeSpan.FromHours(2))).ReusedWithin(ReuseScope.Container);

			}
            else {
				//container.Register<ICacheProvider>(c => new InMemoryCache(TimeSpan.FromHours(2))).ReusedWithin(ReuseScope.Container);
                container.Register<ICacheProvider>(new MemcachedClientCache(new[] { "127.0.0.1" }, ConfigurationManager.AppSettings["redis:namespace"], TimeSpan.FromHours(2))); 

			}
			
		}
    }
}
