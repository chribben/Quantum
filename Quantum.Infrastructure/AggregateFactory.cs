using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonDomain;
using CommonDomain.Persistence;

namespace Quantum.Infrastructure
{
	public class AggregateFactory : IConstructAggregates
	{
		public IAggregate Build(Type type, Guid id, IMemento snapshot)
		{
			return Activator.CreateInstance(type) as IAggregate;
		}
	}
}
