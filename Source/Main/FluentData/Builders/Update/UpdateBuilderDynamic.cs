using System;
using System.Dynamic;

namespace FluentData
{
	internal class UpdateBuilderDynamic : BaseUpdateBuilder, IUpdateBuilderDynamic, IInsertUpdateBuilderDynamic
	{
		public dynamic Item { get; private set; }

		internal UpdateBuilderDynamic(IDbProvider dbProvider, IDbCommand command, string name, ExpandoObject item)
			: base(dbProvider, command, name)
		{
			Data.Item = item;
			Item = item;
		}

		public virtual IUpdateBuilderDynamic Where(string columnName, object value, DataTypes parameterType, int size)
		{
			Actions.WhereAction(columnName, value, parameterType, size);
			return this;
		}

		public IUpdateBuilderDynamic Column(string columnName, object value, DataTypes parameterType, int size)
		{
			Actions.ColumnValueAction(columnName, value, parameterType, size);
			return this;
		}

		public IUpdateBuilderDynamic Column(string propertyName, DataTypes parameterType, int size)
		{
			Actions.ColumnValueDynamic((ExpandoObject) Data.Item, propertyName, parameterType, size);
			return this;
		}

		public IUpdateBuilderDynamic Where(string name, DataTypes parameterType, int size)
		{
			var propertyValue = ReflectionHelper.GetPropertyValueDynamic(Data.Item, name);
			Where(name, propertyValue, parameterType, size);
			return this;
		}

		public IUpdateBuilderDynamic AutoMap(params string[] ignoreProperties)
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

		public IUpdateBuilderDynamic Fill(Action<IInsertUpdateBuilderDynamic> fillMethod)
		{
			fillMethod(this);
			return this;
		}
	}
}
