using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentData
{
	internal class AutoMapper<T>
	{
		private readonly DbCommandData _dbCommandData;
		private readonly Dictionary<string, PropertyInfo> _properties;
		private readonly List<DataReaderField> _fields;
		private readonly System.Data.IDataReader _reader;

		internal AutoMapper(DbCommandData dbCommandData, Type itemType)
		{
			_dbCommandData = dbCommandData;
			_reader = dbCommandData.Reader.InnerReader;
			_properties = ReflectionHelper.GetProperties(itemType);
			_fields = DataReaderHelper.GetDataReaderFields(_reader);
		}

		public void AutoMap(object item)
		{
			foreach (var field in _fields)
			{
				if (field.IsSystem)
					continue;

				var value = _reader.GetValue(field.Index);
				var wasMapped = false;

				PropertyInfo property = null;
					
				if (_properties.TryGetValue(field.LowerName, out property))
				{
					SetPropertyValue(field, property, item, value);
					wasMapped = true;
				}
				else
				{
					if (field.LowerName.IndexOf('_') != -1)
						wasMapped = HandleComplexField(item, field, value);
				}

				if (!wasMapped && !_dbCommandData.Context.Data.IgnoreIfAutoMapFails)
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
				instance = _dbCommandData.Context.Data.EntityFactory.Create(property.PropertyType);

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
