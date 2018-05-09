using StackExchange.Redis;

namespace Custouch.Redis.Interface
{
	public interface IBaseRedis
    {
		string DB_Name { get; set; }

		string ConvertJson<T>(T value);

		T ConvertObj<T>(string json);

		T ConvertObj<T>(RedisValue value);
	}
}
