using System.Collections.Generic;
using System.Dynamic;

namespace FluentData
{
	internal class StoredProcedureBuilderDynamic : BaseStoredProcedureBuilder, IStoredProcedureBuilderDynamic
	{
		internal StoredProcedureBuilderDynamic(IDbProvider dbProvider, IDbCommand command, string name, ExpandoObject item)
			: base(dbProvider, command, name)
		{
			Data.Item = (IDictionary<string, object>) item;
		}

		public IStoredProcedureBuilderDynamic Parameter(string name, object value)
		{
			Actions.ColumnValueAction(name, value, true);
			return this;
		}

		public IStoredProcedureBuilderDynamic IgnoreProperty(string name)
		{
			Actions.AutoMapIgnorePropertyAction(name);
			return this;
		}

		public IStoredProcedureBuilderDynamic AutoMap()
		{
			Actions.AutoMapDynamicTypeColumnsAction(true);
			return this;
		}

		public IStoredProcedureBuilderDynamic ParameterOut(string name, DataTypes parameterType)
		{
			Actions.ParameterOutputAction(name, parameterType);
			return this;
		}
	}
}
