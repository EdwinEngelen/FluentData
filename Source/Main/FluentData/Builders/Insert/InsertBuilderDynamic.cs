using System.Collections.Generic;
using System.Dynamic;

namespace FluentData
{
	internal class InsertBuilderDynamic : BaseInsertBuilder, IInsertBuilderDynamic, IInsertUpdateBuilderDynamic
	{
		internal InsertBuilderDynamic(IDbProvider provider, IDbCommand command, string name, ExpandoObject item)
			: base(provider, command, name)
		{
			Data.Item = (IDictionary<string, object>) item;
		}

		public IInsertBuilderDynamic Column(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value, false);
			return this;
		}

		public IInsertBuilderDynamic Column(string propertyName)
		{
			Actions.ColumnValueDynamic((ExpandoObject) Data.Item, propertyName);
			return this;
		}

		public IInsertBuilderDynamic IgnoreProperty(string name)
		{
			Actions.AutoMapIgnorePropertyAction(name);
			return this;
		}

		public IInsertBuilderDynamic AutoMap()
		{
			Actions.AutoMapDynamicTypeColumnsAction(false);
			return this;
		}

		IInsertUpdateBuilderDynamic IInsertUpdateBuilderDynamic.IgnoreProperty(string name)
		{
			Actions.AutoMapIgnorePropertyAction(name);
			return this;
		}

		IInsertUpdateBuilderDynamic IInsertUpdateBuilderDynamic.AutoMap()
		{
			Actions.AutoMapDynamicTypeColumnsAction(false);
			return this;
		}

		IInsertUpdateBuilderDynamic IInsertUpdateBuilderDynamic.Column(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value, false);
			return this;
		}

		IInsertUpdateBuilderDynamic IInsertUpdateBuilderDynamic.Column(string propertyName)
		{
			Actions.ColumnValueDynamic((ExpandoObject) Data.Item, propertyName);
			return this;
		}
	}
}
