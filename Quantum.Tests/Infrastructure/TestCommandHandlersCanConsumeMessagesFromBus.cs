using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Magnum.Extensions;
using MassTransit;
using MassTransit.Saga;
using MassTransit.SubscriptionConfigurators;
using MassTransit.Testing;
using NUnit.Framework;
using Quantum.Commands;
using Quantum.Domain.CommandHandlers;
using Quantum.Domain.Events;
using Quantum.Infrastructure.Installers;

namespace Quantum.Tests.Infrastructure
{
	[TestFixture]
	public class TestCommandHandlersCanConsumeMessagesFromBus
	{
		readonly IWindsorContainer _container;

		public IServiceBus LocalBus { get; private set; }


		public TestCommandHandlersCanConsumeMessagesFromBus()
		{
			_container = new WindsorContainer().Register(Component.For<MyEventConsumer>());

			_container.Install(
				new CommandHandlerInstaller(),
				new EventPublisherInstaller(),
				new EventStoreInstaller(),
				new RepositoryInstaller(),
				new LocalBusInstaller());
			LocalBus = _container.Resolve<IServiceBus>();
		}

		[Given]
		public void installation_of_infrastructure_objects()
		{
		}

		[Then]
		public void Should_have_a_subscription_for_SeparateDocumentsRequested_event()
		{
			Assert.AreEqual(LocalBus.HasSubscription<SeparateDocumentsRequested>().Count(), 1, "No subscription for the SeparateDocumentsRequested was found.");
		}


		[Then]
		public void Should_have_a_subscription_for_RequestSeparateDocuments_command()
		{
			Assert.AreEqual(LocalBus.HasSubscription<RequestSeparateDocuments>().Count(), 1, "No subscription for the SeparateDocumentsRequested was found.");
		}

		[Then]
		public void Should_have_a_subscription_for_RequestClassifyDocuments_command()
		{
			Assert.AreEqual(
				LocalBus.HasSubscription<RequestClassifyDocuments>().Count(), 1, "No subscription for the ClassifyDocumentsRequested was found.");
		}
		[TestFixtureTearDown]
		public void TearDown()
		{
			_container.Dispose();
		}
	}
}
