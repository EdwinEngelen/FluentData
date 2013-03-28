using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace FluentData
{
	internal class ActionsHandler
	{
		private bool _autoMappedAlreadyCalled = false;

		private readonly BuilderData _data;

		internal ActionsHandler(BuilderData data)
		{
			_data = data;
		}

		internal void ColumnValueAction(string columnName, object value, DataTypes parameterType, int size)
		{
			ColumnAction(columnName, value, typeof(object), parameterType, size);
		}

		private void ColumnAction(string columnName, object value, Type type, DataTypes parameterType, int size)
		{
			var parameterName = columnName;

			_data.Columns.Add(new BuilderColumn(columnName, value, parameterName));

			if(parameterType == DataTypes.Object)
				parameterType = _data.Command.Data.Context.Data.Provider.GetDbTypeForClrType(type);

			ParameterAction(parameterName, value, parameterType, ParameterDirection.Input, size);
		}

		internal void ColumnValueAction<T>(Expression<Func<T, object>> expression, DataTypes parameterType, int size)
		{
			var parser = new PropertyExpressionParser<T>(_data.Item, expression);

			ColumnAction(parser.Name, parser.Value, parser.Type, parameterType, size);
		}

		internal void ColumnValueDynamic(ExpandoObject item, string propertyName, DataTypes parameterType, int size)
		{
			var propertyValue = (item as IDictionary<string, object>) [propertyName];

			ColumnAction(propertyName, propertyValue, typeof(object), parameterType, size);
		}

		private void VerifyAutoMapAlreadyCalled()
		{
			if (_autoMappedAlreadyCalled)
				throw new FluentDataException("AutoMap cannot be called more than once.");
			_autoMappedAlreadyCalled = true;
		}

		internal void AutoMapColumnsAction<T>(params Expression<Func<T, object>>[] ignorePropertyExpressions)
		{
			VerifyAutoMapAlreadyCalled();

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
				ColumnAction(property.Value.Name, propertyValue, propertyType, DataTypes.Object, 0);
			}
		}

		internal void AutoMapDynamicTypeColumnsAction(params string[] ignorePropertyExpressions)
		{
			VerifyAutoMapAlreadyCalled();

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
					ColumnAction(property.Key, property.Value, typeof(object), DataTypes.Object, 0);
			}
		}

		private void ParameterAction(string name, object value, DataTypes dataType, ParameterDirection direction, int size)
		{
			_data.Command.Parameter(name, value, dataType, direction, size);
		}

		internal void ParameterOutputAction(string name, DataTypes dataTypes, int size)
		{
			ParameterAction(name, null, dataTypes, ParameterDirection.Output, size);
		}

		internal void WhereAction(string columnName, object value, DataTypes parameterType, int size)
		{
			var parameterName = columnName;
			ParameterAction(parameterName, value, parameterType, ParameterDirection.Input, 0);

			_data.Where.Add(new BuilderColumn(columnName, value, parameterName));
		}

		internal void WhereAction<T>(Expression<Func<T, object>> expression, DataTypes parameterType, int size)
		{
			var parser = new PropertyExpressionParser<T>(_data.Item, expression);
			WhereAction(parser.Name, parser.Value, parameterType, size);
		}
	}
}
