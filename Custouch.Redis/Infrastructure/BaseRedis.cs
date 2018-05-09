using Custouch.Redis.Interface;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Custouch.Redis.Infrastructure
{
	public class BaseRedis : IBaseRedis
	{
		public virtual string DB_Name { get; set; }

		public string ConvertJson<T>(T value)
		{
			return value is string ? value.ToString() : JsonConvert.SerializeObject(value);
		}
		public T ConvertObj<T>(string json)
		{
			return string.IsNullOrEmpty(json) ? default(T) : JsonConvert.DeserializeObject<T>(json);
		}
		public T ConvertObj<T>(RedisValue value)
		{
			return value.IsNullOrEmpty ? default(T) : JsonConvert.DeserializeObject<T>(value);
		}

	}
}
