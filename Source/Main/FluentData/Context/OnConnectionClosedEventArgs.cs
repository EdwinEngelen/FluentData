using System;
using System.Data;

namespace FluentData
{
	public class OnConnectionClosedEventArgs : EventArgs
	{
		public IDbConnection Connection { get; private set; }

		public OnConnectionClosedEventArgs(System.Data.IDbConnection connection)
		{
			Connection = connection;
		}
	}
}
