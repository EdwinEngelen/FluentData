using System;
using System.Data;

namespace FluentData
{
	public class OnConnectionOpeningEventArgs : EventArgs
	{
		public IDbConnection Connection { get; private set; }

		public OnConnectionOpeningEventArgs(System.Data.IDbConnection connection)
		{
			Connection = connection;
		}
	}
}
