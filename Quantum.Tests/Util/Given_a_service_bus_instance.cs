using MassTransit;
using MassTransit.SubscriptionConfigurators;
using NUnit.Framework;

namespace Quantum.Tests
{
	[TestFixture]
	public abstract class Given_a_service_bus_instance
	{
		protected IServiceBus LocalBus { get; private set; }

		[TestFixtureSetUp]
		public void A_service_bus_instance()
		{
			LocalBus = ServiceBusFactory.New(x =>
			                                 	{
			                                 		x.ReceiveFrom("loopback://localhost/mt_client");

			                                 		x.Subscribe(SubscribeLocalBus);
			                                 	});
		}

		[TestFixtureTearDown]
		public void Finally()
		{
			LocalBus.Dispose();
		}

		protected abstract void SubscribeLocalBus(SubscriptionBusServiceConfigurator subscriptionBusServiceConfigurator);
	}
}