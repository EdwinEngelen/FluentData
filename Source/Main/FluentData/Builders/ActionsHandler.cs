using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace FluentData
{
	internal class ActionsHandler
	{
		private readonly BuilderData _data;

		internal ActionsHandler(BuilderData data)
		{
			_data = data;
		}

		internal void ColumnValueAction(string columnName, object value)
		{
			ColumnAction(columnName, value, typeof(object));
		}

		private void ColumnAction(string columnName, object value, Type type)
		{
			var parameterName = columnName;

			_data.Columns.Add(new TableColumn(columnName, value, parameterName));

			ParameterAction(parameterName, value, DataTypes.Object, ParameterDirection.Input, false);
		}

		internal void ColumnValueAction<T>(Expression<Func<T, object>> expression)
		{
			var parser = new PropertyExpressionParser<T>(_data.Item, expression);

			ColumnAction(parser.Name, parser.Value, parser.Type);
		}

		internal void ColumnValueDynamic(ExpandoObject item, string propertyName)
		{
			var propertyValue = (item as IDictionary<string, object>) [propertyName];

			ColumnAction(propertyName, propertyValue, typeof(object));
		}

		internal void AutoMapColumnsAction<T>(params Expression<Func<T, object>>[] ignorePropertyExpressions)
		{
			var properties = ReflectionHelper.GetProperties(_data.Item.GetType());
			var ignorePropertyNames = new HashSet<string>();
			if (ignorePropertyExpressions != null)
			{
				foreach (var ignorePropertyExpression in ignorePropertyExpressions)
				{
					var ignorePropertyName = new PropertyExpressionParser<T>(_data.Item, ignorePropertyExpression).Name;
					ignorePropertyNames.Add(ignorePropertyName);
				}
			}

			foreach (var property in properties)
			{
				var ignoreProperty = ignorePropertyNames.SingleOrDefault(x => x.Equals(property.Value.Name, StringComparison.CurrentCultureIgnoreCase));
				if (ignoreProperty != null)
					continue;

				var propertyType = ReflectionHelper.GetPropertyType(property.Value);

				var propertyValue = ReflectionHelper.GetPropertyValue(_data.Item, property.Value);
				ColumnAction(property.Value.Name, propertyValue, propertyType);
			}
		}

		internal void AutoMapDynamicTypeColumnsAction(params string[] ignorePropertyExpressions)
		{
			var properties = (IDictionary<string, object>) _data.Item;
			var ignorePropertyNames = new HashSet<string>();
			if (ignorePropertyExpressions != null)
			{
				foreach (var ignorePropertyExpression in ignorePropertyExpressions)
					ignorePropertyNames.Add(ignorePropertyExpression);
			}

			foreach (var property in properties)
			{
				var ignoreProperty = ignorePropertyNames.SingleOrDefault(x => x.Equals(property.Key, StringComparison.CurrentCultureIgnoreCase));

				if (ignoreProperty == null)
					ColumnAction(property.Key, property.Value, typeof(object));
			}
		}

		private void ParameterAction(string name, object value, DataTypes dataType, ParameterDirection direction, bool isId, int size = 0)
		{
			_data.Command.Parameter(name, value, dataType, direction, size);
		}

		internal void ParameterOutputAction(string name, DataTypes dataTypes, int size)
		{
			ParameterAction(name, null, dataTypes, ParameterDirection.Output, false, size);
		}

		internal void WhereAction(string columnName, object value)
		{
			var parameterName = columnName;
			ParameterAction(parameterName, value, DataTypes.Object, ParameterDirection.Input, true);

			_data.Where.Add(new TableColumn(columnName, value, parameterName));
		}

		internal void WhereAction<T>(Expression<Func<T, object>> expression)
		{
			var parser = new PropertyExpressionParser<T>(_data.Item, expression);
			WhereAction(parser.Name, parser.Value);
		}
	}
}
