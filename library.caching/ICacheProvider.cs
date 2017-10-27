using System;
namespace library.caching
{
	/// <summary>
	/// Provides a generic intrface wrapper for various ServiceStack cache clients
	/// </summary>
	public interface ICacheProvider
	{
		/// <summary>
		/// AddOrUpdate or update an item in the cache
		/// </summary>
		bool AddOrUpdate<T>(string key, T entity);

		/// <summary>
		/// AddOrUpdate or update an item in the cache
		/// </summary>
		bool AddOrUpdate<T>(string key, T entity, TimeSpan timeSpan);

		/// <summary>
		/// Get an item from the cache
		/// </summary>
		T Get<T>(string key);

		/// <summary>
		/// Invalidate an item in th cache
		/// </summary>
		bool Remove<T>(string key);

		/// <summary>
		/// Completly clear all caches
		/// </summary>
		void Clear();
	}
}
