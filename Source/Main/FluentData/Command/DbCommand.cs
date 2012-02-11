using System;
using System.Text;

namespace FluentData
{
	internal partial class DbCommand : IDisposable, IDbCommand
	{
		private readonly DbCommandData _data;

		public DbCommand(
			DbContext dbContext,
			System.Data.IDbCommand dbCommand,
			DbContextData dbContextData)
		{
			_data = new DbCommandData();
			_data.DbContext = dbContext;
			_data.DbContextData = dbContextData;
			_data.InnerCommand = dbCommand;
			_data.Command = this;
			_data.QueryExecuter = new QueryExecuter(_data);
		}

		internal IDbCommand UseMultipleResultset
		{
			get
			{
				if (!_data.DbContextData.DbProvider.SupportsMultipleResultset)
					throw new FluentDbException("The selected database does not support multiple resultset");

				_data.MultipleResultset = true;
				return this;
			}
		}

		public IDbCommand CommandType(DbCommandTypes dbCommandType)
		{
			_data.DbCommandType = dbCommandType;
			_data.InnerCommand.CommandType = (System.Data.CommandType) dbCommandType;
			return this;
		}

		public void Dispose()
		{
			if (_data.Reader != null)
				_data.Reader.Close();

			if (!_data.DbContextData.UseTransaction)
				_data.InnerCommand.Connection.Close();
		}
	}
}
