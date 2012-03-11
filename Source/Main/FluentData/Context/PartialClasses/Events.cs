using System;

namespace FluentData
{
	public partial class DbContext : IDbContext
	{
		public IDbContext OnConnectionOpening(Action<OnConnectionOpeningEventArgs> action)
		{
			ContextData.OnConnectionOpening = action;
			return this;
		}

		public IDbContext OnConnectionOpened(Action<OnConnectionOpenedEventArgs> action)
		{
			ContextData.OnConnectionOpened = action;
			return this;
		}

		public IDbContext OnConnectionClosed(Action<OnConnectionClosedEventArgs> action)
		{
			ContextData.OnConnectionClosed = action;
			return this;
		}

		public IDbContext OnExecuting(Action<OnExecutingEventArgs> action)
		{
			ContextData.OnExecuting = action;
			return this;
		}

		public IDbContext OnExecuted(Action<OnExecutedEventArgs> action)
		{
			ContextData.OnExecuted = action;
			return this;
		}

		public IDbContext OnError(Action<OnErrorEventArgs> action)
		{
			ContextData.OnError = action;
			return this;
		}
	}
}
