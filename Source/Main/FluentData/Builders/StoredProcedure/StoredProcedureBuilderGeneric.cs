using System;
using System.Linq.Expressions;

namespace FluentData
{
	internal class StoredProcedureBuilder<T> : BaseStoredProcedureBuilder, IStoredProcedureBuilder<T>
	{
		internal StoredProcedureBuilder(IDbCommand command, string name, T item)
			: base(command, name)
		{
			Data.Item = item;
		}

		public IStoredProcedureBuilder<T> Parameter(string name, object value)
		{
			Actions.ColumnValueAction(name, value);
			return this;
		}

		public IStoredProcedureBuilder<T> AutoMap(params Expression<Func<T, object>>[] ignoreProperties)
		{
			Actions.AutoMapColumnsAction(ignoreProperties);
			return this;
		}

		public IStoredProcedureBuilder<T> Parameter(Expression<Func<T, object>> expression)
		{
			Actions.ColumnValueAction(expression);

			return this;
		}

		public IStoredProcedureBuilder<T> ParameterOut(string name, DataTypes parameterType, int size = 0)
		{
			Actions.ParameterOutputAction(name, parameterType, size);
			return this;
		}
	}
}
