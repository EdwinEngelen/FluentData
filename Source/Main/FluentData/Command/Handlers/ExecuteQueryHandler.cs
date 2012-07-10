using System;
using System.Data;

namespace FluentData
{
	internal class ExecuteQueryHandler
	{
		private readonly DbCommandData _data;
		private readonly DbCommand _command;
		private bool _queryAlreadyExecuted;

		public ExecuteQueryHandler(DbCommandData data, DbCommand command)
		{
			_data = data;
			_command = command;
		}

		internal void ExecuteQuery(bool useReader, Action action)
		{
			try
			{
				PrepareDbCommand(useReader);

				action();

				if (_data.ContextData.OnExecuted != null)
					_data.ContextData.OnExecuted(new OnExecutedEventArgs(_data.InnerCommand));
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
				if (_data.UseMultipleResultsets)
					_data.Reader.NextResult();
				else
					throw new FluentDataException("A query has already been executed on this command object. Please create a new command object.");
			}
			else
			{
				_data.InnerCommand.CommandText = _data.Sql.ToString();
				if (_data.ContextData.CommandTimeout != Int32.MinValue)
					_data.InnerCommand.CommandTimeout = _data.ContextData.CommandTimeout;

				if (_data.ContextData.UseTransaction)
				{
					if (_data.ContextData.Transaction == null)
					{
						OpenConnection();
						_data.ContextData.Transaction = _data.ContextData.Connection.BeginTransaction((System.Data.IsolationLevel) _data.ContextData.IsolationLevel);
					}
					_data.InnerCommand.Transaction = _data.ContextData.Transaction;
				}
				else
				{
					if (_data.InnerCommand.Connection.State != ConnectionState.Open)
						OpenConnection();
				}

				if (_data.ContextData.OnExecuting != null)
					_data.ContextData.OnExecuting(new OnExecutingEventArgs(_data.InnerCommand));
				
				if (useReader)
					_data.Reader = new DataReader(_data.InnerCommand.ExecuteReader());

				_queryAlreadyExecuted = true;
			}
		}

		private void OpenConnection()
		{
			if (_data.ContextData.OnConnectionOpening != null)
				_data.ContextData.OnConnectionOpening(new OnConnectionOpeningEventArgs(_data.InnerCommand.Connection));

			_data.InnerCommand.Connection.Open();

			if (_data.ContextData.OnConnectionOpened != null)
				_data.ContextData.OnConnectionOpened(new OnConnectionOpenedEventArgs(_data.InnerCommand.Connection));
		}

		private void HandleQueryFinally()
		{
			if (!_data.UseMultipleResultsets)
			{
				if (_data.Reader != null)
					_data.Reader.Close();

				_command.ClosePrivateConnection();
			}
		}

		private void HandleQueryException(Exception exception)
		{
			if (_data.Reader != null)
				_data.Reader.Close();

			_command.ClosePrivateConnection();
			if (_data.ContextData.UseTransaction)
				_data.Context.CloseSharedConnection();

			if (_data.ContextData.OnError != null)
				_data.ContextData.OnError(new OnErrorEventArgs(_data.InnerCommand, exception));
			
			throw exception;
		}
	}
}
