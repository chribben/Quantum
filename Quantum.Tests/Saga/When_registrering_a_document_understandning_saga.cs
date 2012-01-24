using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using MassTransit;
using MassTransit.Saga;
using MassTransit.SubscriptionConfigurators;
using MassTransit.Testing;
using NUnit.Framework;
using Quantum.Domain.Domain;
using Quantum.Domain.Events;

namespace Quantum.Tests.Saga
{
	[TestFixture]
	public class When_registrering_a_document_understandning_saga :
		Given_a_service_bus_instance
	{
		readonly IWindsorContainer _container;

		public When_registrering_a_document_understandning_saga()
		{
			_container = new WindsorContainer();
			_container.Register(
				Component.For<DocumentUnderstandingSaga>(),
				Component.For(typeof(ISagaRepository<>))
					.ImplementedBy(typeof(InMemorySagaRepository<>))
					.LifeStyle.Singleton);
		}


		[Then]
		public void Should_have_a_subscription_for_SeparateDocumentsRequested()
		{
			Assert.AreEqual(LocalBus.HasSubscription<SeparateDocumentsRequested>().Count(), 1, "No subscription for the SeparateDocumentsRequested was found.");
		}

		[Then]
		public void Should_have_a_subscription_for_SeparateDocumentsCompleted()
		{
			Assert.AreEqual(
				LocalBus.HasSubscription<SeparateDocumentsCompleted>().Count(), 1, "No subscription for the SeparateDocumentsCompleted was found.");
		}
		[Then]
		public void Should_have_a_subscription_for_the_third_saga_message()
		{
			Assert.AreEqual(
				LocalBus.HasSubscription<ClassifyDocumentsRequested>().Count(), 1, "No subscription for the ClassifyDocumentsRequested was found.");
		}
		[Then]
		public void Should_have_a_subscription_for_the_fourth_saga_message()
		{
			Assert.AreEqual(
				LocalBus.HasSubscription<ClassifyDocumentsCompleted>().Count(), 1, "No subscription for the ClassifyDocumentsCompleted was found.");
		}

		[TestFixtureTearDown]
		public void Close_container()
		{
			_container.Dispose();
		}

		protected override void SubscribeLocalBus(SubscriptionBusServiceConfigurator subscriptionBusServiceConfigurator)
		{
			subscriptionBusServiceConfigurator.LoadFrom(_container);
		}
	}
}
