using System.Collections.Generic;
using System.Dynamic;

namespace FluentData
{
	internal class UpdateBuilderDynamic : BaseUpdateBuilder, IUpdateBuilderDynamic, IInsertUpdateBuilderDynamic
	{
		internal UpdateBuilderDynamic(IDbProvider dbProvider, IDbCommand command, string name, ExpandoObject item)
			: base(dbProvider, command, name)
		{
			Data.Item = (IDictionary<string, object>) item;
		}

		public virtual IUpdateBuilderDynamic Where(string columnName, object value)
		{
			Actions.WhereAction(columnName, value);
			return this;
		}

		public IUpdateBuilderDynamic Column(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value);
			return this;
		}

		public IUpdateBuilderDynamic Column(string propertyName)
		{
			Actions.ColumnValueDynamic((ExpandoObject) Data.Item, propertyName);
			return this;
		}

		public IUpdateBuilderDynamic Where(string name)
		{
			var propertyValue = ReflectionHelper.GetPropertyValueDynamic(Data.Item, name);
			Where(name, propertyValue);
			return this;
		}

		public IUpdateBuilderDynamic AutoMap(params string[] ignoreProperties)
		{
			Actions.AutoMapDynamicTypeColumnsAction(ignoreProperties);
			return this;
		}

		IInsertUpdateBuilderDynamic IInsertUpdateBuilderDynamic.AutoMap(params string[] ignoreProperties)
		{
			Actions.AutoMapDynamicTypeColumnsAction(ignoreProperties);
			return this;
		}

		IInsertUpdateBuilderDynamic IInsertUpdateBuilderDynamic.Column(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value);
			return this;
		}

		IInsertUpdateBuilderDynamic IInsertUpdateBuilderDynamic.Column(string propertyName)
		{
			Actions.ColumnValueDynamic((ExpandoObject) Data.Item, propertyName);
			return this;
		}
	}
}
