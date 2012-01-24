using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonDomain;
using CommonDomain.Core;
using CommonDomain.Persistence;
using Quantum.Domain.Events;
using Quantum.EventStore;

namespace Quantum.Repository
{
	public class SimpleEventRepository : IRepository
	{
		private readonly IEventStore _eventStore;
		private readonly IConstructAggregates _factory;

		public SimpleEventRepository(IEventStore eventStore,
			IConstructAggregates factory)
		{
			_eventStore = eventStore;
			_factory = factory;
		}


		TAggregate IRepository.GetById<TAggregate>(Guid id)
		{
			var aggregate = _factory.Build(typeof(TAggregate), id, null);
			var events = _eventStore.GetEventsForAggregate(id);
			ApplyEventsToAggregate(events, aggregate);
			return aggregate as TAggregate;
		}

		private void ApplyEventsToAggregate(IEnumerable<DomainEvent> events, IAggregate aggregate)
		{
			foreach (var domainEvent in events)
			{
				aggregate.ApplyEvent(domainEvent);
			}
		}

		public TAggregate GetById<TAggregate>(Guid id, int version) where TAggregate : class, IAggregate
		{
			throw new NotImplementedException();
		}

		public void Save(IAggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders)
		{
			_eventStore.SaveEvents(aggregate.Id, aggregate.GetUncommittedEvents());
		}
	}
}
