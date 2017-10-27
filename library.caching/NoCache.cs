using System;
namespace library.caching
{
	/// <summary>
	/// Provides a null (no cache) implementation
	/// </summary>
	public class NoCache : ICacheProvider
	{
		/// <summary>
		/// AddOrUpdate or update an item in the cache
		/// </summary>
		public bool AddOrUpdate<T>(string key, T entity)
		{
			return false;
		}
		/// <summary>
		/// AddOrUpdate or update an item in the cache
		/// </summary>
		public bool AddOrUpdate<T>(string key, T entity, TimeSpan timespan)
		{
			return false;
		}

		/// <summary>
		/// Get an item from the cache
		/// </summary>
		public T Get<T>(string key)
		{
			return default(T);
		}

		/// <summary>
		/// Invalidate an item in th cache
		/// </summary>
		public bool Remove<T>(string key)
		{
			return false;
		}

		/// <summary>
		/// Completly clear all caches
		/// </summary>
		public void Clear()
		{
		}
	}
}