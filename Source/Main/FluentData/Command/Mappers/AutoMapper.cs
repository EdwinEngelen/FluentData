using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentData
{
	internal class AutoMapper<T> : BaseMapper<AutoMapper<T>>
	{
		private readonly DbCommandData _dbCommandData;
		private readonly Dictionary<Type, List<PropertyInfo>> _cachedProperties;

		internal AutoMapper(DbCommandData dbCommandData)
		{
			_dbCommandData = dbCommandData;
			_cachedProperties = new Dictionary<Type, List<PropertyInfo>>();
			Reader(dbCommandData.Reader);
		}

		public void AutoMap(object item)
		{
			var properties = GetProperties(item);
			foreach (var field in Fields)
			{
				var value = base._reader.GetValue(field.Index);
				bool wasMapped;

				if (IsComplex(field, properties))
					wasMapped = HandleComplexField(0, item, field, value);
				else
					wasMapped = HandleSimpleField(item, field, value);

				if (!wasMapped && !_dbCommandData.ContextData.IgnoreIfAutoMapFails)
					throw new FluentDataException("Could not map: " + field.Name);
			}
		}

		private bool IsComplex(DataReaderField field, List<PropertyInfo> properties)
		{
			foreach (var property in properties)
			{
				if (property.Name.Equals(field.Name, StringComparison.CurrentCultureIgnoreCase))
					return false;
			}

			if (field.Name.Contains("_"))
				return true;

			return false;
		}

		private bool HandleSimpleField(object item, DataReaderField field, object value)
		{
			var properties = GetProperties(item);

			foreach (var property in properties)
			{
				if (property.Name.Equals(field.Name, StringComparison.CurrentCultureIgnoreCase))
				{
					SetPropertyValue(field, property, item, value);
					return true;
				}
			}

			return false;
		}

		private bool HandleComplexField(int level, object item, DataReaderField field, object value)
		{
			var propertyName = field.GetNestedName(level);

			var properties = GetProperties(item);

			var property = properties.SingleOrDefault(x => x.Name.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase));

			if (property != null)
			{
				if (level == field.NestedLevels)
				{
					SetPropertyValue(field, property, item, value);
					return true;
				}
				else
				{
					object instance = GetOrCreateInstance(item, property);

					if (instance == null)
						return false;

					return HandleComplexField(level + 1, instance, field, value);
				}
			}

			return false;
		}

		private object GetOrCreateInstance(object item, PropertyInfo property)
		{
			object instance = ReflectionHelper.GetPropertyValue(item, property);

			if (instance == null)
			{
				instance = _dbCommandData.ContextData.EntityFactory.Create(property.PropertyType);

				property.SetValue(item, instance, null);
			}

			return instance;
		}

		private List<PropertyInfo> GetProperties(object item)
		{
			var type = item.GetType();

			if (_cachedProperties.ContainsKey(type))
				return _cachedProperties[type];

			var properties = ReflectionHelper.GetProperties(item);
			_cachedProperties.Add(type, properties);

			return properties;
		}

		private void SetPropertyValue(DataReaderField field, PropertyInfo property, object item, object value)
		{
			try
			{
				if (value == DBNull.Value)
				{
					if (ReflectionHelper.IsNullable(property))
						value = null;
					else
						value = ReflectionHelper.GetDefault(property.PropertyType);
				}
				else
				{
					var propertyType = ReflectionHelper.GetPropertyType(property);

					if (propertyType != field.Type)
					{
						if (propertyType.IsEnum)
						{
							if (field.Type == typeof(string))
								value = Enum.Parse(propertyType, (string) value, true);
							else
								value = Enum.ToObject(propertyType, value);
						}
						else if (!ReflectionHelper.IsBasicClrType(propertyType))
							return;
						else if (propertyType == typeof(string))
							value = value.ToString();
						else
							value = Convert.ChangeType(value, property.PropertyType);
					}
				}

				property.SetValue(item, value, null);
			}
			catch (Exception exception)
			{
				throw new FluentDataException("Could not map: " + property.Name, exception);
			}
		}	
	}
}
