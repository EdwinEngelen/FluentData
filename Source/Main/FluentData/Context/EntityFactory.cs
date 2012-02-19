using System;

namespace FluentData
{
	internal class EntityFactory : IEntityFactory
	{
		public virtual object Create(Type type)
		{
			return Activator.CreateInstance(type);
		}
	}
}
