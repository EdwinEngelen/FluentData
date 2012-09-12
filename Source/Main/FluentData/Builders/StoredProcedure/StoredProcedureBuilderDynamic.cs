using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;

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
			Actions.ColumnValueAction(name, value);
			return this;
		}

		public IStoredProcedureBuilderDynamic AutoMap(params string[] ignoreProperties)
		{
			Actions.AutoMapDynamicTypeColumnsAction(ignoreProperties);
			return this;
		}

		public IStoredProcedureBuilderDynamic ParameterOut(string name, DataTypes parameterType, int size = 0)
		{
			Actions.ParameterOutputAction(name, parameterType, size);
			return this;
		}
	}
}
