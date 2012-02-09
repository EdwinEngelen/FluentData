using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentData
{
	internal class AutoMapper<T> : BaseMapper<AutoMapper<T>>
	{
		private readonly DbContextData _dbContextData;
		private readonly Dictionary<Type, List<PropertyInfo>> _cachedProperties;

		internal IEntityFactory EntityFactory { get; set; }

		internal AutoMapper(DbContextData dbContextData)
		{
			_dbContextData = dbContextData;
			_cachedProperties = new Dictionary<Type, List<PropertyInfo>>();
		}

		public void AutoMap(object item)
		{
			foreach (var field in Fields)
			{
				var isDbNull = base._reader.IsDBNull(field.Index);
				var value = base._reader.GetValue(field.Index);
				bool wasMapped;

				if (field.IsComplex)
					wasMapped = HandleComplexField(0, item, field, value);
				else
					wasMapped = HandleSimpleField(item, field, value);

				if (!wasMapped && _dbContextData.ThrowExceptionIfAutoMapFails)
					throw new FluentDbException("Could not map: " + field.Name);
			}
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
				instance = EntityFactory.Resolve(property.PropertyType);

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
						if (!ReflectionHelper.IsBasicClrType(propertyType))
							return;
						else if (propertyType == typeof(string))
							value = value.ToString();
						else if (propertyType.IsEnum)
							value = Enum.ToObject(propertyType, value);
						else
							value = Convert.ChangeType(value, property.PropertyType);
					}
				}

				property.SetValue(item, value, null);
			}
			catch (Exception exception)
			{
				throw new FluentDbException("Could not map: " + property.Name, exception);
			}
		}	
	}
}
