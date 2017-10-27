using System;
using ServiceStack;
using ServiceStack.Redis;

namespace library.caching
{
	/// <summary>
	/// Provides a wrapper around the Service Stack InMemory cache client
	/// </summary>
	public class RedisCacheClient : ICacheProvider
	{
		private readonly PooledRedisClientManager _clientManager;
		private readonly string _ns;
		private readonly TimeSpan _defaulTimespan;

		/// <summary>
		/// Constructor
		/// </summary>
		public RedisCacheClient(string connectionString, string ns, TimeSpan defaultTimespan)
		{
			_clientManager = new PooledRedisClientManager(connectionString);
			_ns = string.IsNullOrEmpty(ns) ? "" : ns + ":";
			_defaulTimespan = defaultTimespan;
		}

		/// <summary>
		/// Add or update an item in the cache
		/// </summary>
		public bool AddOrUpdate<T>(string key, T entity)
		{
			using (var cacheClient = _clientManager.GetCacheClient())
			{
				return cacheClient.Set(_ns + key, entity, _defaulTimespan);
			}
		}
		/// <summary>
		/// Add or update an item in the cache
		/// </summary>
		public bool AddOrUpdate<T>(string key, T entity, TimeSpan timespan)
		{
			using (var cacheClient = _clientManager.GetCacheClient())
			{
				// don't expire for MaxValue
				if (timespan == TimeSpan.MaxValue)
				{
					return cacheClient.Set(_ns + key, entity);
				}
				return cacheClient.Set(_ns + key, entity, timespan);
			}
		}

		/// <summary>
		/// Get an item from the cache
		/// </summary>
		public T Get<T>(string key)
		{
			using (var cacheClient = _clientManager.GetReadOnlyCacheClient())
			{
				return cacheClient.Get<T>(_ns + key);
			}
		}

		/// <summary>
		/// Invalidate an item in th cache
		/// </summary>
		public bool Remove<T>(string key)
		{
			using (var cacheClient = _clientManager.GetCacheClient())
			{
				return cacheClient.Remove(_ns + key);
			}
		}

		/// <summary>
		/// Completly clear all caches
		/// </summary>
		public void Clear()
		{
			using (var cacheClient = _clientManager.GetCacheClient())
			{
				cacheClient.ClearCaches();
			}
		}
	}
}