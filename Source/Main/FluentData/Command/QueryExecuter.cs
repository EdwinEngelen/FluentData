using System;

namespace FluentData
{
	internal class QueryExecuter
	{
		private readonly DbCommandData _data;
		private bool _queryAlreadyExecuted;

		public QueryExecuter(DbCommandData data)
		{
			_data = data;
		}

		internal void ExecuteQueryHandler(bool useReader, Action action)
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
				if (_data.MultipleResultset)
					_data.Reader.NextResult();
				else
					throw new FluentDbException("A query has already been executed on this command object. Please create a new command object.");
			}
			else
			{
				FixSql();
				FixParameterType();

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
			_data.DbContextData.DbProvider.PrepareCommandBeforeExecute(_data);
		}

		private void HandleQueryFinally()
		{
			if (!_data.MultipleResultset)
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

		private void FixParameterType()
		{
			foreach (var parameter in _data.Parameters)
			{
				if (parameter.Direction == ParameterDirection.Input
					&& parameter.DataTypes == DataTypes.Object)
				{
					if (parameter.Value == null)
						parameter.Value = DBNull.Value;
					else
					{
						parameter.DataTypes = _data.DbContextData.DbProvider.GetDbTypeForClrType(parameter.Value.GetType());
						if (parameter.DataTypes == DataTypes.Object)
							throw new FluentDbException(string.Format("The parameter {0} is off a type that is not supported.", parameter.ParameterName));
					}
				}

				var dbParameter = _data.InnerCommand.CreateParameter();
				dbParameter.DbType = (System.Data.DbType) parameter.DataTypes;
				dbParameter.ParameterName = _data.DbContextData.DbProvider.GetParameterName(parameter.ParameterName);
				dbParameter.Direction = (System.Data.ParameterDirection) parameter.Direction;
				dbParameter.Value = parameter.Value;
				_data.InnerCommand.Parameters.Add(dbParameter);
			}
		}
	}
}
