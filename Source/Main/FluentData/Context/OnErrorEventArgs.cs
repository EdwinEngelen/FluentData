using System;

namespace FluentData
{
	public class OnErrorEventArgs : EventArgs
	{
		public System.Data.IDbCommand Command { get; private set; }
		public Exception Exception { get; set; }

		public OnErrorEventArgs(System.Data.IDbCommand command, Exception exception)
		{
			Command = command;
			Exception = exception;
		}
	}
}
