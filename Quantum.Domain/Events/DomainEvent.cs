using System;

namespace Quantum.Domain.Events
{
	public abstract class DomainEvent
	{
		protected DomainEvent()
		{
			
		}
		protected DomainEvent(Guid aggregateId, int aggregateVersion)
		{
			AggregateId = aggregateId;
			Version = aggregateVersion;
		}

		public Guid AggregateId { get; protected set; }
		public int Version { get; protected set; }
	}
}