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
using Quantum.EventStore;
using Quantum.Infrastructure;
using Quantum.Infrastructure.Installers;
using Quantum.Repository;
using log4net;
using log4net.Config;

namespace Quantum.Tests.Infrastructure
{
	[TestFixture]
	public class TestSendMessageReceiveEventNoIoC
	{
		private readonly Guid _aggregateId = Guid.NewGuid();
		private static readonly ILog Logger = LogManager.GetLogger(typeof (TestSendMessageReceiveEvent));
		private readonly ManualResetEvent _received = new ManualResetEvent(false);

		public TestSendMessageReceiveEventNoIoC()
		{
			BasicConfigurator.Configure();
		}

		public IServiceBus LocalBus { get; private set; }

		[Given]
		public void installation_of_infrastructure_objects()
		{
			Logger.Info("Installing stuff");
			LocalBus = ServiceBusFactory.New(x => x.ReceiveFrom("loopback://localhost/mt_client"));
			var eventPublisher = new EventPublisher(LocalBus);
			var inMemoryEventStore = new InMemoryEventStore(eventPublisher);
			var aggregateFactory = new AggregateFactory();
			var simpleEventRepository = new SimpleEventRepository(inMemoryEventStore, aggregateFactory);
			LocalBus.SubscribeConsumer(() => new MyEventConsumer(_received));
			LocalBus.SubscribeConsumer(() => new CreateBatchCommandHandler(simpleEventRepository));
			LocalBus.SubscribeConsumer(() => new RequestSeparateDocumentsCommandHandler(simpleEventRepository));
			LocalBus.SubscribeConsumer(() => new RequestClassifyDocumentsCommandHandler(simpleEventRepository));


			//LocalBus.WriteIntrospectionToFile("d:/ws/programmering/quantum/quantum.tests/localbuslog.txt");
			//PipelineViewer.Trace(LocalBus.InboundPipeline);
			//PipelineViewer.Trace(LocalBus.OutboundPipeline);
			//Prep the event store for this aggregate (could've published a CreateBatch command, but then we would be testing that as well)
			simpleEventRepository.Save(new Batch(_aggregateId), Guid.NewGuid(), null);
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
			_received.WaitOne(10000).ShouldBeTrue();
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			LocalBus.Dispose();
		}
	}
}