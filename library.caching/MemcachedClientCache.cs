using System;
using System.Collections.Generic;
using System.Net;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;

namespace library.caching
{
    public class MemcachedClientCache : ICacheProvider
    {
		private MemcachedClient _client;
		private readonly string _ns;
		private readonly TimeSpan _defaulTimespan;

		/// <summary>
		/// Initializes the Cache using the provided hosts to configure the memcached client
		/// </summary>
		/// <param name="hosts"></param>
		public MemcachedClientCache(IEnumerable<string> hosts, string ns, TimeSpan defaultTimespan)
		{
			_defaulTimespan = defaultTimespan;
			_ns = string.IsNullOrEmpty(ns) ? "" : ns + ":";
			const int defaultPort = 8080;
			const int ipAddressIndex = 0;
			const int portIndex = 1;

			var ipEndpoints = new List<IPEndPoint>();
			foreach (var host in hosts)
			{
				var hostParts = host.Split(':');
				if (hostParts.Length == 0)
					throw new ArgumentException("'{0}' is not a valid host IP Address: e.g. '127.0.0.0[:11211]'");

				var port = (hostParts.Length == 1) ? defaultPort : int.Parse(hostParts[portIndex]);

				var hostAddresses = Dns.GetHostAddresses(hostParts[ipAddressIndex]);
				foreach (var ipAddress in hostAddresses)
				{
					var endpoint = new IPEndPoint(ipAddress, port);
					ipEndpoints.Add(endpoint);
				}
			}

			LoadClient(PrepareMemcachedClientConfiguration(ipEndpoints));
		}

		public MemcachedClientCache(IEnumerable<IPEndPoint> ipEndpoints)
		{
			LoadClient(PrepareMemcachedClientConfiguration(ipEndpoints));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MemcachedClientCache"/> class based on an existing <see cref="IMemcachedClientConfiguration"/>.
		/// </summary>
		/// <param name="memcachedClientConfiguration">The <see cref="IMemcachedClientConfiguration"/>.</param>
		public MemcachedClientCache(IMemcachedClientConfiguration memcachedClientConfiguration)
		{
			LoadClient(memcachedClientConfiguration);
		}

		/// <summary>
		/// Prepares a MemcachedClientConfiguration based on the provided ipEndpoints.
		/// </summary>
		/// <param name="ipEndpoints">The ip endpoints.</param>
		/// <returns></returns>
		private IMemcachedClientConfiguration PrepareMemcachedClientConfiguration(IEnumerable<IPEndPoint> ipEndpoints)
		{
			var config = new MemcachedClientConfiguration();
			foreach (var ipEndpoint in ipEndpoints)
			{
				config.Servers.Add(ipEndpoint);
			}

			config.SocketPool.MinPoolSize = 10;
			config.SocketPool.MaxPoolSize = 100;
			config.SocketPool.ConnectionTimeout = new TimeSpan(0, 0, 10);
			config.SocketPool.DeadTimeout = new TimeSpan(0, 2, 0);

			return config;
		}

		private void LoadClient(IMemcachedClientConfiguration config)
		{
			_client = new MemcachedClient(config);
		}


        public bool AddOrUpdate<T>(string key, T entity)
		{
			return _client.Store(StoreMode.Add, _ns + key, entity, _defaulTimespan);
        }

        public bool AddOrUpdate<T>(string key, T entity, TimeSpan timeSpan)
		{
            
			if (timeSpan == TimeSpan.MaxValue)
			{
				return _client.Store(StoreMode.Add, _ns + key, entity);
			}
			return _client.Store(StoreMode.Add, _ns + key, entity, timeSpan);
        }

        public void Clear()
        {
            _client.FlushAll();
        }

        public T Get<T>(string key)
		{
			return _client.Get<T>(_ns + key);
        }

        public bool Remove<T>(string key)
        {
            return _client.Remove(key);
        }
    }
}
