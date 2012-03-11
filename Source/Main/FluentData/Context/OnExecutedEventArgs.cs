using System;

namespace FluentData
{
	public class OnExecutedEventArgs : EventArgs
	{
		public System.Data.IDbCommand Command { get; private set; }

		public OnExecutedEventArgs(System.Data.IDbCommand command)
		{
			Command = command;
		}
	}
}
