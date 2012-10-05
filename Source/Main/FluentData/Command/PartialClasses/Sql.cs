namespace FluentData
{
	internal partial class DbCommand
	{
		public IDbCommand Sql(string sql)
		{
			Data.InnerCommand.CommandText += sql;
			return this;
		}
	}
}
