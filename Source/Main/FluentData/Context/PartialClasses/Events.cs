using System;

namespace FluentData
{
	public partial class DbContext
	{
		public IDbContext OnConnectionOpening(Action<ConnectionEventArgs> action)
		{
			Data.OnConnectionOpening = action;
			return this;
		}

        public IDbContext OnConnectionOpened(Action<ConnectionEventArgs> action)
		{
			Data.OnConnectionOpened = action;
			return this;
		}

		public IDbContext OnConnectionClosed(Action<ConnectionEventArgs> action)
		{
			Data.OnConnectionClosed = action;
			return this;
		}

		public IDbContext OnExecuting(Action<CommandEventArgs> action)
		{
			Data.OnExecuting = action;
			return this;
		}

		public IDbContext OnExecuted(Action<CommandEventArgs> action)
		{
			Data.OnExecuted = action;
			return this;
		}

		public IDbContext OnError(Action<ErrorEventArgs> action)
		{
			Data.OnError = action;
			return this;
		}
	}
}
