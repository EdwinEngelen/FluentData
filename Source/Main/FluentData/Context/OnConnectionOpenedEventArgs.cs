using System;
using System.Data;

namespace FluentData
{
	public class OnConnectionOpenedEventArgs : EventArgs
	{
		public IDbConnection Connection { get; private set; }

		public OnConnectionOpenedEventArgs(System.Data.IDbConnection connection)
		{
			Connection = connection;
		}
	}
}
