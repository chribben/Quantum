using EventStore;
using EventStore.Dispatcher;
using Magnum.Reflection;
using MassTransit;

namespace Quantum.EventStore
{
	public class MassTransitPublisher : IDispatchCommits
	{
		private readonly IServiceBus _Bus;

		public MassTransitPublisher(IServiceBus bus)
		{
			_Bus = bus;
		}

		void IDispatchCommits.Dispatch(Commit commit)
		{
			commit.Events.ForEach(@event => this.FastInvoke("PublishEvent", @event.Body));
		}

		void PublishEvent<T>(T message)
			where T : class
		{
			_Bus.Publish(message);
		}

		public void Dispose()
		{
			_Bus.Dispose();
		}
	}
}