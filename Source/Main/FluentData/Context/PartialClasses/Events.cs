using System;

namespace FluentData
{
	public partial class DbContext : IDbContext
	{
		public IDbContext OnConnectionOpening(Action<OnConnectionOpeningEventArgs> action)
		{
			Data.OnConnectionOpening = action;
			return this;
		}

		public IDbContext OnConnectionOpened(Action<OnConnectionOpenedEventArgs> action)
		{
			Data.OnConnectionOpened = action;
			return this;
		}

		public IDbContext OnConnectionClosed(Action<OnConnectionClosedEventArgs> action)
		{
			Data.OnConnectionClosed = action;
			return this;
		}

		public IDbContext OnExecuting(Action<OnExecutingEventArgs> action)
		{
			Data.OnExecuting = action;
			return this;
		}

		public IDbContext OnExecuted(Action<OnExecutedEventArgs> action)
		{
			Data.OnExecuted = action;
			return this;
		}

		public IDbContext OnError(Action<OnErrorEventArgs> action)
		{
			Data.OnError = action;
			return this;
		}
	}
}
