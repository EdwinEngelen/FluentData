using System.Collections.Generic;
using System.Linq;

namespace FluentData
{
	public class ParameterCollection : List<Parameter>
	{
		public Parameter this[string name]
		{
			get
			{
				return this.SingleOrDefault(x => x.ParameterName == name);
			}
		}
	}
}
