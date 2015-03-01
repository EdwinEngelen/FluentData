using System;

namespace FluentData
{
	internal class StoredProcedureBuilder : BaseStoredProcedureBuilder, IStoredProcedureBuilder
	{
		internal StoredProcedureBuilder(IDbCommand command, string name)
			: base(command, name)
		{
		}

		public IStoredProcedureBuilder Parameter(string name, object value, DataTypes parameterType, int size)
		{
			Actions.ColumnValueAction(name, value, parameterType, size);
			return this;
		}

		public IStoredProcedureBuilder ParameterOut(string name, DataTypes parameterType, int size = 0)
		{
			Actions.ParameterOutputAction(name, parameterType, size);
			return this;
		}

		public IStoredProcedureBuilder UseMultiResult(bool useMultipleResultsets)
		{
			Data.Command.UseMultiResult(useMultipleResultsets);
			return this;
		}
	}	
}
