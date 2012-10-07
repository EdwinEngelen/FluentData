using System.Collections.Generic;
using System.Dynamic;

namespace FluentData
{
	internal class InsertBuilderDynamic : BaseInsertBuilder, IInsertBuilderDynamic, IInsertUpdateBuilderDynamic
	{
		internal InsertBuilderDynamic(IDbCommand command, string name, ExpandoObject item)
			: base(command, name)
		{
			Data.Item = (IDictionary<string, object>) item;
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
