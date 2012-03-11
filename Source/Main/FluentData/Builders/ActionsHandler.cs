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

		internal void ColumnValueAction(string columnName, object value, bool propertyNameIsParameterName)
		{
			ColumnAction(columnName, false, value, propertyNameIsParameterName);
		}

		private void ColumnAction(string columnName, bool isSql, object value, bool propertyNameIsParameterName)
		{
			var parameterName = "";
			if (propertyNameIsParameterName)
				parameterName = columnName;
			else
				parameterName = "c" + _data.Columns.Count.ToString();

			_data.Columns.Add(new TableColumn(columnName, value, parameterName));

			if (!isSql)
				ParameterAction(parameterName, value, DataTypes.Object, ParameterDirection.Input, false);
		}

		internal void ColumnValueAction<T>(Expression<Func<T, object>> expression, bool propertyNameIsParameterName)
		{
			var propertyName = ReflectionHelper.GetPropertyNameFromExpression(expression);

			ColumnAction(propertyName, false, ReflectionHelper.GetPropertyValue(_data.Item, propertyName), propertyNameIsParameterName);
		}

		internal void ColumnValueDynamic(ExpandoObject item, string propertyName)
		{
			var propertyValue = (item as IDictionary<string, object>) [propertyName];

			ColumnAction(propertyName, false, propertyValue, true);
		}

		internal void AutoMapColumnsAction<T>(bool propertyNameIsParameterName, params Expression<Func<T, object>>[] ignorePropertyExpressions)
		{
			var properties = ReflectionHelper.GetProperties(_data.Item);
			var ignorePropertyNames = new HashSet<string>();
			if (ignorePropertyExpressions != null)
			{
				foreach (var ignorePropertyExpression in ignorePropertyExpressions)
				{
					var ignorePropertyName = ReflectionHelper.GetPropertyNameFromExpression(ignorePropertyExpression);
					ignorePropertyNames.Add(ignorePropertyName);
				}
			}

			foreach (var property in properties)
			{
				var propertyType = ReflectionHelper.GetPropertyType(property);

				var ignoreProperty = ignorePropertyNames.SingleOrDefault(x => x.Equals(property.Name, StringComparison.CurrentCultureIgnoreCase));

				if (ignoreProperty == null
					&& ReflectionHelper.IsBasicClrType(propertyType))
				{
					ColumnAction(property.Name, false, ReflectionHelper.GetPropertyValue(_data.Item, property), propertyNameIsParameterName);
				}
			}
		}

		internal void AutoMapDynamicTypeColumnsAction(bool propertyNameIsParameterName, params string[] ignorePropertyExpressions)
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

				if (ignoreProperty == null
					&& ReflectionHelper.IsBasicClrType(property.Value.GetType()))
				{
					ColumnAction(property.Key, false, property.Value, propertyNameIsParameterName);
				}
			}
		}

		internal void ParameterAction(string name, object value, DataTypes dataTypes, ParameterDirection direction, bool isId, int size = 0)
		{
			var parameter = new Parameter();
			parameter.ParameterName = name;
			parameter.Value = value;
			parameter.DataTypes = dataTypes;
			parameter.Direction = direction;
			parameter.IsId = isId;
			parameter.Size = size;

			_data.Parameters.Add(parameter);
			_data.Command.Parameter(parameter.ParameterName, parameter.Value, parameter.DataTypes, parameter.Direction);
		}

		internal void ParameterOutputAction(string name, DataTypes dataTypes, int size)
		{
			ParameterAction(name, null, dataTypes, ParameterDirection.Output, false, size);
		}

		internal void ParametersAction(object[] parameters)
		{
			var count = parameters.Count();

			for (int i = 0; i < count; i++)
				ParameterAction(i.ToString(), parameters[i], DataTypes.Object, ParameterDirection.Input, false);
		}

		internal void WhereAction(string columnName, object value)
		{
			var parameterName = "id" + _data.Where.Count().ToString();
			ParameterAction(parameterName, value, DataTypes.Object, ParameterDirection.Input, true);

			_data.Where.Add(new TableColumn(columnName, value, parameterName));
		}

		internal void WhereAction<T>(Expression<Func<T, object>> expression)
		{
			var propertyName = ReflectionHelper.GetPropertyNameFromExpression(expression);
			var propertyValue = ReflectionHelper.GetPropertyValue(_data.Item, propertyName);
			WhereAction(propertyName, propertyValue);
		}
	}
}
