using Custouch.Redis.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml;

namespace Custouch.Redis.RedisManager
{
	public class RedisManager : IConfigurationSectionHandler
	{
		private static Dictionary<string, RedisConfig> DicHosts = new Dictionary<string, RedisConfig>();

		static RedisManager()
		{
			DicHosts = new Dictionary<string, RedisConfig>();
			var configList = ConfigurationManager.GetSection("Redis.Service") as List<RedisConfig>;
			foreach (var item in configList)
			{
				DicHosts.Add(item.DbName, item);
			}
		}

		public static RedisConfig GetRedisConfig(string dbName)
		{
			if (DicHosts.Keys.Where(p => p == dbName).Count() > 0)
			{
				return DicHosts[dbName];
			}
			throw new Exception("数据库不存在");
		}

		public object Create(object parent, object configContext, XmlNode section)
		{
			var list = new List<RedisConfig>();
			for (int i = 0; i < section.ChildNodes.Count; i++)
			{
				var item = section.ChildNodes[i];
				var obj = new RedisConfig(item.Attributes["DbHosts"].Value, Convert.ToInt32(item.Attributes["DbNum"].Value)) { DbName = item.Attributes["Name"].Value };
				list.Add(obj);
			}
			return list;
		}

	}
}
