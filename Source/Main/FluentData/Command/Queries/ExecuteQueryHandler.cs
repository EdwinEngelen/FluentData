using System;
using System.Data;

namespace FluentData
{
	internal class ExecuteQueryHandler
	{
		private readonly DbCommand _command;
		private bool _queryAlreadyExecuted;

		public ExecuteQueryHandler(DbCommand command)
		{
			_command = command;
		}

		internal void ExecuteQuery(bool useReader, Action action)
		{
			try
			{
				PrepareDbCommand(useReader);

				action();

				if (_command.Data.Context.Data.OnExecuted != null)
					_command.Data.Context.Data.OnExecuted(new OnExecutedEventArgs(_command.Data.InnerCommand));
			}
			catch (Exception exception)
			{
			    HandleQueryException(exception);
			}
			finally
			{
			    HandleQueryFinally();
			}
		}

		private void PrepareDbCommand(bool useReader)
		{
			if (_queryAlreadyExecuted)
			{
				if(_command.Data.UseMultipleResultsets)
					_command.Data.Reader.NextResult();
				else
					throw new FluentDataException("A query has already been executed on this command object. Please create a new command object.");
			}
			else
			{
				if(_command.Data.Context.Data.CommandTimeout != Int32.MinValue)
					_command.Data.InnerCommand.CommandTimeout = _command.Data.Context.Data.CommandTimeout;

				if(_command.Data.InnerCommand.Connection.State != ConnectionState.Open)
					OpenConnection();

				if(_command.Data.Context.Data.UseTransaction)
				{
					if(_command.Data.Context.Data.Transaction == null)
						_command.Data.Context.Data.Transaction = _command.Data.Context.Data.Connection.BeginTransaction((System.Data.IsolationLevel)_command.Data.Context.Data.IsolationLevel);

					_command.Data.InnerCommand.Transaction = _command.Data.Context.Data.Transaction;
				}

				if(_command.Data.Context.Data.OnExecuting != null)
					_command.Data.Context.Data.OnExecuting(new OnExecutingEventArgs(_command.Data.InnerCommand));

				if (useReader)
					_command.Data.Reader = new DataReader(_command.Data.InnerCommand.ExecuteReader());

				_queryAlreadyExecuted = true;
			}
		}

		private void OpenConnection()
		{
			if(_command.Data.Context.Data.OnConnectionOpening != null)
				_command.Data.Context.Data.OnConnectionOpening(new OnConnectionOpeningEventArgs(_command.Data.InnerCommand.Connection));

			_command.Data.InnerCommand.Connection.Open();

			if(_command.Data.Context.Data.OnConnectionOpened != null)
				_command.Data.Context.Data.OnConnectionOpened(new OnConnectionOpenedEventArgs(_command.Data.InnerCommand.Connection));
		}

		private void HandleQueryFinally()
		{
			if(!_command.Data.UseMultipleResultsets)
			{
				if(_command.Data.Reader != null)
					_command.Data.Reader.Close();

				_command.ClosePrivateConnection();
			}
		}

		private void HandleQueryException(Exception exception)
		{
			if(_command.Data.Reader != null)
				_command.Data.Reader.Close();

			_command.ClosePrivateConnection();
			if(_command.Data.Context.Data.UseTransaction)
				_command.Data.Context.CloseSharedConnection();

			if(_command.Data.Context.Data.OnError != null)
				_command.Data.Context.Data.OnError(new OnErrorEventArgs(_command.Data.InnerCommand, exception));
			
			throw exception;
		}
	}
}
