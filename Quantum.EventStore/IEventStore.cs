using System;
using System.Collections;
using System.Collections.Generic;
using Quantum.Domain.Events;

namespace Quantum.EventStore
{
	public interface IEventStore
	{
		IEnumerable<DomainEvent> GetEventsForAggregate(Guid id);
		void SaveEvents(Guid id, ICollection uncommittedEvents);
	}
}