using System;
using ServiceStack;
using ServiceStack.Caching;

namespace library.caching
{
	/// <summary>
	/// Provides a wrapper around the Service Stack InMemory cache client
	/// </summary>
	public class InMemoryCache : ICacheProvider
	{
		private readonly ICacheClient _cacheClient;
		private readonly TimeSpan _defaulTimespan;

		/// <summary>
		/// Constructor
		/// </summary>
		public InMemoryCache(TimeSpan defaultTimespan)
		{
			_cacheClient = new MemoryCacheClient();
			_defaulTimespan = defaultTimespan;
		}

		/// <summary>
		/// AddOrUpdate or update an item in the cache
		/// </summary>
		public bool AddOrUpdate<T>(string key, T entity)
		{
			return _cacheClient.Add(key, entity, _defaulTimespan);
		}

		/// <summary>
		/// AddOrUpdate or update an item in the cache
		/// </summary>
		public bool AddOrUpdate<T>(string key, T entity, TimeSpan timeSpan)
		{
			// don't expire for MaxValue
			if (timeSpan == TimeSpan.MaxValue)
			{
				return _cacheClient.Add(key, entity);
			}
			return _cacheClient.Add(key, entity, timeSpan);
		}

		/// <summary>
		/// Get an item from the cache
		/// </summary>
		public T Get<T>(string key)
		{
			return _cacheClient.Get<T>(key);
		}

		/// <summary>
		/// Invalidate an item in th cache
		/// </summary>
		public bool Remove<T>(string key)
		{
			return _cacheClient.Remove(key);
		}

		/// <summary>
		/// Completly clear all caches
		/// </summary>
		public void Clear()
		{
			_cacheClient.ClearCaches();
		}
	}
}
