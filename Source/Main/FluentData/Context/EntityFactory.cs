using System;

namespace FluentData
{
	internal class EntityFactory : IEntityFactory
	{
		public virtual object Resolve(Type type)
		{
			return Activator.CreateInstance(type);
		}
	}
}
