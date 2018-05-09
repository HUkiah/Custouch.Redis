using Custouch.Redis.Infrastructure;
using StackExchange.Redis;
using System;

namespace Custouch.Redis.Utils
{
	public abstract class RedisQueue<T>:BaseRedis
    {
		protected virtual bool IsLock { get; set; }

		protected virtual string Key { get; set; }


		/// <summary>
		/// 入队
		/// </summary>
		/// <param name="t"></param>
		public void Push(T t)
		{
			Push(Key, t);
		}

		/// <summary>
		/// 将指定的值插入到存储在键的列表尾部
		/// </summary>
		/// <param name="key"></param>
		/// <param name="t"></param>
		private void Push(string key, T t)
		{
			var redisConfig = RedisManager.RedisManager.GetRedisConfig(DB_Name);
			var lockdb = redisConfig.GetDatabase(-1);
			var db = redisConfig.GetDatabase();

			if (IsLock)
			{
				var token = Environment.MachineName;
				if (lockdb.LockTake(key, token, TimeSpan.FromSeconds(20)))
				{
					try
					{
						db.ListRightPush(key, ConvertJson(t));
					}
					finally
					{
						lockdb.LockRelease(key, token);
					}
				}
			}
			else
			{
				db.ListRightPush(key, ConvertJson(t));
			}

		}

		/// <summary>
		/// 出队
		/// </summary>
		/// <returns></returns>
		public T Pop()
		{
			var keyInfo = Key;
			var redisConfig = RedisManager.RedisManager.GetRedisConfig(DB_Name);
			var lockdb = redisConfig.GetDatabase(-1);
			var db = redisConfig.GetDatabase();
			if (IsLock)
			{
				var token = Environment.MachineName;
				if (lockdb.LockTake(keyInfo, token, TimeSpan.FromSeconds(20)))
				{
					try
					{
						var json = db.ListLeftPop(keyInfo);
						if (json == default(RedisValue))
						{
							return default(T);
						}
						return ConvertObj<T>(json);
					}
					finally
					{
						lockdb.LockRelease(keyInfo, token);
					}
				}
				return default(T);
			}
			else
			{
				var json = db.ListLeftPop(keyInfo);
				if (json == default(RedisValue))
				{
					return default(T);
				}
				return ConvertObj<T>(json);
			}
		}

		/// <summary>
		/// 数量
		/// </summary>
		/// <returns></returns>
		public int Count()
		{
			var redisConfig = RedisManager.RedisManager.GetRedisConfig(DB_Name);
			var db = redisConfig.GetDatabase();
			var len = db.ListLength(Key);

			return Convert.ToInt32(len);
		}
	}
}
