using System;
using System.Data;

namespace FluentData
{
	internal class ExecuteQueryHandler
	{
		private readonly DbCommandData _data;
		private bool _queryAlreadyExecuted;

		public ExecuteQueryHandler(DbCommandData data)
		{
			_data = data;
		}

		internal void ExecuteQuery(bool useReader, Action action)
		{
			try
			{
				PrepareDbCommand(useReader);
				action();
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
				FixSql();
				new ParameterHandler().FixParameterType(_data);

				if (_data.DbContextData.UseTransaction)
				{
					if (_data.DbContextData.Transaction == null)
					{
						_data.InnerCommand.Connection.Open();
						_data.DbContextData.Transaction = _data.DbContextData.Connection.BeginTransaction((System.Data.IsolationLevel) _data.DbContextData.IsolationLevel);
					}
					_data.InnerCommand.Transaction = _data.DbContextData.Transaction;
				}
				else
					_data.InnerCommand.Connection.Open();

				if (useReader)
					_data.Reader = new DataReader(_data.InnerCommand.ExecuteReader());

				_queryAlreadyExecuted = true;
			}
		}

		private void HandleQueryFinally()
		{
			if (!_data.UseMultipleResultsets)
			{
				if (_data.Reader != null)
					_data.Reader.Close();

				if (!_data.DbContextData.UseTransaction)
					_data.InnerCommand.Connection.Close();
			}
		}

		private void HandleQueryException(Exception exception)
		{
			if (_data.Reader != null)
				_data.Reader.Close();

			if (_data.DbContextData.UseTransaction)
				_data.DbContext.Rollback();

			_data.InnerCommand.Connection.Close();

			throw exception;
		}

		private void FixSql()
		{
			_data.DbContextData.DbProvider.FixInStatement(_data.Sql, _data.Parameters);
			_data.InnerCommand.CommandText = _data.Sql.ToString();
		}
	}
}
