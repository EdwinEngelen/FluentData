using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentData
{
	internal class DataReaderField
	{
		public int Index { get; set; }
		public string Name { get; set; }
		public Type Type { get; set; }
		private List<string> _nestedPropertyNames;

		public bool IsComplex
		{
			get
			{
				return Name.Contains("_");
			}
		}

		public string GetNestedName(int level)
		{
			if (_nestedPropertyNames == null)
			{
				_nestedPropertyNames = Name.Split('_').ToList();
			}

			return _nestedPropertyNames[level];
		}

		public int NestedLevels
		{
			get
			{
				return _nestedPropertyNames.Count - 1;
			}
		}
	}
}
