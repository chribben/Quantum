using Quantum.Domain.Events;

namespace Quantum.EventStore
{
	public interface IEventPublisher
	{
		void PublishEvent(DomainEvent @event);
	}
}