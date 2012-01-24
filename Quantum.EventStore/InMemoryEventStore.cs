using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantum.Domain.Events;

namespace Quantum.EventStore
{
	public class InMemoryEventStore : IEventStore
	{
		private readonly IEventPublisher _publisher;
		readonly Dictionary<Guid, List<DomainEvent>> _eventsPerAggregate = new Dictionary<Guid, List<DomainEvent>>();

		public InMemoryEventStore(IEventPublisher publisher)
		{
			_publisher = publisher;
		}

		public IEnumerable<DomainEvent> GetEventsForAggregate(Guid id)
		{
			return _eventsPerAggregate[id];
		}

		public void SaveEvents(Guid id, ICollection uncommittedEvents)
		{
			List<DomainEvent> eventList;
			if (!_eventsPerAggregate.TryGetValue(id, out eventList))
			{
				eventList = new	List<DomainEvent>();
				_eventsPerAggregate.Add(id, eventList);
			}
			foreach (var domainEvent in uncommittedEvents)
			{
				var @event = domainEvent as DomainEvent;
				eventList.Add(@event);
				_publisher.PublishEvent(@event);
			}
		}
	}
}
