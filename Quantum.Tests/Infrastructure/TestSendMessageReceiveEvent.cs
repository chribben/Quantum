using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonDomain.Persistence;
using Machine.Specifications;
using MassTransit;
using MassTransit.Pipeline.Inspectors;
using MassTransit.Testing;
using NUnit.Framework;
using Quantum.Commands;
using Quantum.Domain.CommandHandlers;
using Quantum.Domain.Domain;
using Quantum.Domain.Events;
using Quantum.Infrastructure.Installers;
using log4net;
using log4net.Config;

namespace Quantum.Tests.Infrastructure
{
	[TestFixture]
	public class TestSendMessageReceiveEvent
	{
		private IWindsorContainer _container;
		private readonly Guid _aggregateId = Guid.NewGuid();
		private static readonly ILog _Logger = LogManager.GetLogger(typeof (TestSendMessageReceiveEvent));
		private readonly ManualResetEvent _received = new ManualResetEvent(false);

		public TestSendMessageReceiveEvent()
		{
			BasicConfigurator.Configure();
		}

		public IServiceBus LocalBus { get; private set; }

		[Given]
		public void installation_of_infrastructure_objects()
		{
			_Logger.Info("Installing stuff");
			_container =
				new WindsorContainer().Register(Component.For<ManualResetEvent>().Instance(_received));
			_container.Register(
					Component.For<MyEventConsumer>().LifeStyle.Singleton);
			_container.Install(
				new CommandHandlerInstaller(),
				new EventPublisherInstaller(),
				new EventStoreInstaller(),
				new RepositoryInstaller(),
				new LocalBusInstaller());
			LocalBus = _container.Resolve<IServiceBus>();
			PipelineViewer.Trace(LocalBus.InboundPipeline);
			//Prep the event store for this aggregate (could've published a CreateBatch command, but then we would be testing that as well)
			var repo = _container.Resolve<IRepository>();
			repo.Save(new Batch(_aggregateId), Guid.NewGuid(), null);
		}

		[When]
		public void sending_a_command()
		{
			LocalBus.Publish(new RequestSeparateDocuments(_aggregateId));
		}
			
		[Then]
		public void corresponding_event_should_be_received_by_consumer()
		{
			Assert.AreEqual(LocalBus.HasSubscription<SeparateDocumentsRequested>().Count(), 1, "No subscription for the SeparateDocumentsRequested was found.");
			_received.WaitOne(5000).ShouldBeTrue();
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			_container.Dispose();
			LocalBus.Dispose();
		}
	}
}