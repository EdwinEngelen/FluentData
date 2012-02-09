using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentData
{
	internal static class ReflectionHelper
	{
		public static object GetPropertyValueFromExpression<T, TProp>(object item, Expression<Func<T, TProp>> expression)
		{
			var propertyPath = expression.Body.ToString().Replace(expression.Parameters[0] + ".", string.Empty);

			foreach (var part in propertyPath.Split('.'))
			{
				if (item == null)
					return null;

				var type = item.GetType();

				var property = type.GetProperty(part);
				if (property == null)
					return null;

				item = GetPropertyValue(item, property);
			}

			return item;
		}

		public static string GetPropertyNameFromExpression<T, TProp>(Expression<Func<T, TProp>> expression)
		{
			var propertyPath = expression.Body.ToString().Replace(expression.Parameters[0] + ".", string.Empty);

			return propertyPath;
		}

		public static object GetPropertyValue(object item, PropertyInfo property)
		{
			return property.GetValue(item, null);
		}

		public static object GetPropertyValue(object item, string propertyName)
		{
			PropertyInfo property;
			foreach (var part in propertyName.Split('.'))
			{
				if (item == null)
					return null;

				var type = item.GetType();

				property = type.GetProperty(part);
				if (property == null)
					return null;

				item = GetPropertyValue(item, property);
			}
			return item;
		}

		public static object GetPropertyValueDynamic(object item, string name)
		{
			var dictionary = (IDictionary<string, object>) item;

			return dictionary[name];
		}
		
		public static List<PropertyInfo> GetProperties(object item)
		{
			return item.GetType().GetProperties().ToList();
		}

		public static bool IsList(object item)
		{
			if (item is ICollection)
				return true;

			return false;
		}

		public static bool IsNullable(PropertyInfo property)
		{
			if (property.PropertyType.IsGenericType &&
				property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
				return true;

			return false;
		}

		/// <summary>
		/// Includes a work around for getting the actual type of a Nullable type.
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		public static Type GetPropertyType(PropertyInfo property)
		{
			if (IsNullable(property))
				return property.PropertyType.GetGenericArguments()[0];

			return property.PropertyType;
		}

		public static object GetDefault(Type type)
		{
			if (type.IsValueType)
				return Activator.CreateInstance(type);
			return null;
		}

		public static bool IsBasicClrType(Type type)
		{
			var types = new HashSet<Type>();
			types.Add(typeof(bool));
			types.Add(typeof(byte));
			types.Add(typeof(long));
			types.Add(typeof(char));
			types.Add(typeof(string));
			types.Add(typeof(DateTime));
			types.Add(typeof(decimal));
			types.Add(typeof(double));
			types.Add(typeof(float));
			types.Add(typeof(Guid));
			types.Add(typeof(short));
			types.Add(typeof(int));

			if (types.Contains(type))
				return true;

			return false;
		}
	}
}
