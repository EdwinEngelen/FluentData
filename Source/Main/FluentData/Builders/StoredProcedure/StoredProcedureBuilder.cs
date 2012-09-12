using System;

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
			Actions.ColumnValueAction(name, value);
			return this;
		}

		public IStoredProcedureBuilder ParameterOut(string name, DataTypes parameterType, int size = 0)
		{
			Actions.ParameterOutputAction(name, parameterType, size);
			return this;
		}
	}	
}
