using System;
using System.Dynamic;

namespace FluentData
{
	internal class InsertBuilderDynamic : BaseInsertBuilder, IInsertBuilderDynamic, IInsertUpdateBuilderDynamic
	{
		public dynamic Item { get; private set; }

		internal InsertBuilderDynamic(IDbCommand command, string name, ExpandoObject item)
			: base(command, name)
		{
			Data.Item = item;
			Item = item;
		}

		public IInsertBuilderDynamic Column(string columnName, object value, DataTypes parameterType, int size)
		{
			Actions.ColumnValueAction(columnName, value, parameterType, size);
			return this;
		}

		public IInsertBuilderDynamic Column(string propertyName, DataTypes parameterType, int size)
		{
			Actions.ColumnValueDynamic((ExpandoObject)Data.Item, propertyName, parameterType, size);
			return this;
		}

		public IInsertBuilderDynamic AutoMap(params string[] ignoreProperties)
		{
			Actions.AutoMapDynamicTypeColumnsAction(ignoreProperties);
			return this;
		}

		public IInsertBuilderDynamic Fill(Action<IInsertUpdateBuilderDynamic> fillMethod)
		{
			fillMethod(this);
			return this;
		}

		IInsertUpdateBuilderDynamic IInsertUpdateBuilderDynamic.Column(string columnName, object value, DataTypes parameterType, int size)
		{
			Actions.ColumnValueAction(columnName, value, parameterType, size);
			return this;
		}

		IInsertUpdateBuilderDynamic IInsertUpdateBuilderDynamic.Column(string propertyName, DataTypes parameterType, int size)
		{
			Actions.ColumnValueDynamic((ExpandoObject)Data.Item, propertyName, parameterType, size);
			return this;
		}
	}
}
