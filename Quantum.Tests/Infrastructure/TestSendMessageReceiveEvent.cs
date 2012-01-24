using System;
using System.Collections.Generic;
using System.Threading;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonDomain.Persistence;
using Machine.Specifications;
using MassTransit;
using MassTransit.Pipeline.Inspectors;
using NUnit.Framework;
using Quantum.Commands;
using Quantum.Domain.CommandHandlers;
using Quantum.Domain.Domain;
using Quantum.Infrastructure.Installers;
using log4net;
using log4net.Config;

namespace Quantum.Tests.Infrastructure
{
	[TestFixture]
	public class TestSendMessageReceiveEvent
	{
		private IWindsorContainer _container;
		private readonly List<ICommandHandler<Command>> _commandHandlers = new List<ICommandHandler<Command>>();
		private readonly MyEventConsumer _myConsumer = new MyEventConsumer(new ManualResetEvent(false));
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
				new WindsorContainer().Register(
					Component.For<MyEventConsumer>().UsingFactoryMethod(() => new MyEventConsumer(_received)));
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
			_received.WaitOne(10000).ShouldBeTrue();
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			_container.Dispose();
			LocalBus.Dispose();
		}
	}
}