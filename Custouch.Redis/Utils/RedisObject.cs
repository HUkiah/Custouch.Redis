using Custouch.Redis.Infrastructure;
using System;

namespace Custouch.Redis.Utils
{
	public abstract class RedisObject<T>:BaseRedis
    {
		public void Add(string key, T t, TimeSpan? span = default(TimeSpan?))
		{
			string json = ConvertJson(t);

			var RedConfig = RedisManager.RedisManager.GetRedisConfig(DB_Name);
			var db = RedConfig.GetDatabase();

			db.StringSet(key, json, span);
		}
		public T Get(string key)
		{

			var redisConfig = RedisManager.RedisManager.GetRedisConfig(DB_Name);
			var db = redisConfig.GetDatabase();

			return ConvertObj<T>(db.StringGet(key));
		}
		public void Remove(string key)
		{
			var redisConfig = RedisManager.RedisManager.GetRedisConfig(DB_Name);
			var db = redisConfig.GetDatabase();
			db.KeyDelete(key);
		}
		public bool IsExist(string key)
		{
			var redisConfig = RedisManager.RedisManager.GetRedisConfig(DB_Name);
			var db = redisConfig.GetDatabase();
			return db.KeyExists(key);
		}
	}
}
