using System;
using System.Linq;

namespace FluentData
{
	internal class DataReaderField
	{
		public int Index { get; private set; }
		public string LowerName { get; private set; }
		public string Name { get; private set; }
		public Type Type { get; private set; }
		private readonly string[] _nestedPropertyNames;
		private readonly int _nestedLevels;

		public DataReaderField(int index, string name, Type type)
		{
			Index = index;
			Name = name;
			LowerName = name.ToLower();
			Type = type;
			_nestedPropertyNames = LowerName.Split('_');
			_nestedLevels = _nestedPropertyNames.Count() - 1;
		}

		public string GetNestedName(int level)
		{
			return _nestedPropertyNames[level];
		}

		public int NestedLevels
		{
			get
			{
				return _nestedLevels;
			}
		}

		public bool IsSystem
		{
			get
			{
				return Name.IndexOf("FLUENTDATA_") > -1;
			}
		}
	}
}
