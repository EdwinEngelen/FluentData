namespace FluentData
{
	internal class StoredProcedureBuilder : BaseStoredProcedureBuilder, IStoredProcedureBuilder
	{
		internal StoredProcedureBuilder(IDbProvider dbProvider, IDbCommand command, string name)
			: base(dbProvider, command, name)
		{
		}

		public IStoredProcedureBuilder Parameter(string name, object value)
		{
			Actions.ColumnValueAction(name, value, true);
			return this;
		}

		public IStoredProcedureBuilder ParameterOut(string name, DataTypes parameterType)
		{
			Actions.ParameterOutputAction(name, parameterType);
			return this;
		}
	}
}
