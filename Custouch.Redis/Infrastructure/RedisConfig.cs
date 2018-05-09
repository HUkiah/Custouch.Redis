using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Custouch.Redis.Infrastructure
{
	public class RedisConfig
	{
		public string DbName { get; set; }

		public string DbHosts { get; set; }

		public int DbNum { get; set; }

		public ConnectionMultiplexer Connection { get; set; }


		public RedisConfig(string dbHosts, int Num = 0)
		{
			DbHosts = dbHosts;
			DbNum = Num;

			GetConnectionMultiplexer();

		}

		public IDatabase GetDatabase(int? num = null)
		{
			if (Connection != null)
			{
				return Connection.GetDatabase(num ?? DbNum, null);
			}
			return null;
		}


		private void GetConnectionMultiplexer()
		{
			Connection = GetConnect(DbHosts);
			Connection.ConnectionFailed += Connection_ConnectionFailed;
			Connection.ErrorMessage += Connection_ErrorMessage;
			Connection.InternalError += Connection_InternalError;
		}

		private ConnectionMultiplexer GetConnect(string connectionString = null)
		{
			if (string.IsNullOrEmpty(connectionString))
			{
				connectionString = "127.0.0.1:6379,allowAdmin=true";
			}

			return ConnectionMultiplexer.Connect(connectionString);
		}

		private void Connection_InternalError(object sender, InternalErrorEventArgs e)
		{
			throw new Exception("redis 内部错误" + e.Exception.Message);
		}



		/// <summary>
		/// 服务器以错误消息回应;
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Connection_ErrorMessage(object sender, RedisErrorEventArgs e)
		{
			throw new Exception(string.Format("redis 异常，{0}", e.Message));
		}


		/// <summary>
		/// 当物理连接失败时，它会被触发
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Connection_ConnectionFailed(object sender, ConnectionFailedEventArgs e)
		{
			throw new Exception(e.Exception.Message);
		}

	}
}
