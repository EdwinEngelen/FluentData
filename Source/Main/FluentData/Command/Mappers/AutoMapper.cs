using System;
using System.Reflection;

namespace FluentData
{
	internal class AutoMapper<T>
	{
		private readonly DbCommandData _dbCommandData;

		internal AutoMapper(DbCommandData dbCommandData)
		{
			_dbCommandData = dbCommandData;
		}

		public void AutoMap(object item)
		{
			var properties = ReflectionHelper.GetProperties(item.GetType());
			var fields = DataReaderHelper.GetDataReaderFields(_dbCommandData.Reader);

			foreach (var field in fields)
			{
				if (field.IsSystem)
					continue;

				var value = _dbCommandData.Reader.GetValue(field.Index);
				var wasMapped = false;

				PropertyInfo property = null;
					
				if (properties.TryGetValue(field.LowerName, out property))
				{
					SetPropertyValue(field, property, item, value);
					wasMapped = true;
				}
				else
				{
					if (field.LowerName.IndexOf('_') != -1)
						wasMapped = HandleComplexField(item, field, value);
				}

				if (!wasMapped && !_dbCommandData.ContextData.IgnoreIfAutoMapFails)
					throw new FluentDataException("Could not map: " + field.Name);
			}
		}

		private bool HandleComplexField(object item, DataReaderField field, object value)
		{
			string propertyName = null;

			for (var level = 0; level <= field.NestedLevels; level++)
			{
				if (string.IsNullOrEmpty(propertyName))
					propertyName = field.GetNestedName(level);
				else
					propertyName += "_" + field.GetNestedName(level);

				PropertyInfo property = null;
				var properties = ReflectionHelper.GetProperties(item.GetType());
				if (properties.TryGetValue(propertyName, out property))
				{
					if (level == field.NestedLevels)
					{
						SetPropertyValue(field, property, item, value);
						return true;
					}
					else
					{
						item = GetOrCreateInstance(item, property);
						if (item == null)
							return false;
						propertyName = null;	
					}
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
