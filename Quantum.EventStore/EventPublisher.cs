using MassTransit;
using Quantum.Domain.Events;

namespace Quantum.EventStore
{
	public class EventPublisher : IEventPublisher
	{
		private readonly IServiceBus _serviceBus;

		public EventPublisher(IServiceBus serviceBus)
		{
			_serviceBus = serviceBus;
		}

		public void PublishEvent(DomainEvent @event)
		{
			_serviceBus.Publish(@event);
		}
	}
}