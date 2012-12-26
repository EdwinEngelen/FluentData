using System;

namespace FluentData
{
	public class ErrorEventArgs : EventArgs
	{
		public System.Data.IDbCommand Command { get; private set; }
		public Exception Exception { get; set; }

		public ErrorEventArgs(System.Data.IDbCommand command, Exception exception)
		{
			Command = command;
			Exception = exception;
		}
	}
}
