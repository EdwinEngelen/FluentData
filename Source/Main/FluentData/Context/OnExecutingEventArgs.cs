using System;

namespace FluentData
{
	public class OnExecutingEventArgs : EventArgs
	{
		public System.Data.IDbCommand Command { get; private set; }

		public OnExecutingEventArgs(System.Data.IDbCommand command)
		{
			Command = command;
		}
	}
}
