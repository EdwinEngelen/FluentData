using System;
using System.Linq.Expressions;

namespace FluentData
{
	internal class StoredProcedureBuilder<T> : BaseStoredProcedureBuilder, IStoredProcedureBuilder<T>
	{
		internal StoredProcedureBuilder(IDbProvider dbProvider, IDbCommand command, string name, T item)
			: base(dbProvider, command, name)
		{
			Data.Item = item;
		}

		public IStoredProcedureBuilder<T> Parameter(string name, object value)
		{
			Actions.ColumnValueAction(name, value, true);
			return this;
		}

		public IStoredProcedureBuilder<T> AutoMap()
		{
			Actions.AutoMapColumnsAction(true);
			return this;
		}

		public IStoredProcedureBuilder<T> IgnoreProperty<TProp>(Expression<Func<T, TProp>> expression)
		{
			Actions.AutoMapIgnorePropertyAction<T, TProp>(expression);
			return this;
		}

		public IStoredProcedureBuilder<T> Parameter<TProp>(Expression<Func<T, TProp>> expression)
		{
			Actions.ColumnValueAction(expression, true);

			return this;
		}

		public IStoredProcedureBuilder<T> ParameterOut(string name, DataTypes parameterType)
		{
			Actions.ParameterOutputAction(name, parameterType);
			return this;
		}
	}
}
