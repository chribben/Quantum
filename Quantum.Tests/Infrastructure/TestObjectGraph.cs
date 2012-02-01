using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Windsor;
using CommonDomain.Persistence;
using Machine.Specifications;
using MassTransit;
using MassTransit.SubscriptionConfigurators;
using NUnit.Framework;
using Quantum.Commands;
using Quantum.Domain.CommandHandlers;
using Quantum.EventStore;
using Quantum.Infrastructure.Installers;
using Quantum.Repository;

namespace Quantum.Tests.Infrastructure
{

	[TestFixture]
	public class TestObjectGraph
	{
		IWindsorContainer _container;
		private IEventStore _eventStore;
		private IEventPublisher _eventPublisher;
		private IRepository _repository;


		[Given]
		public void installation_of_infrastructure_objects()
		{
			_container = new WindsorContainer().Install(
				new CommandHandlerInstaller(),
				new EventPublisherInstaller(),
				new EventStoreInstaller(),
				new RepositoryInstaller(),
				new LocalBusInstaller());
		}

		[When]
		public void resolving_objects()
		{
			_eventStore = _container.Resolve<IEventStore>();
			_eventPublisher = _container.Resolve<IEventPublisher>();
			_repository = _container.Resolve<IRepository>();
			//_commandHandlers.AddRange(new List<Consumes<Command>.All> { _container.Resolve<Consumes<RequestClassifyDocuments>.All>(), _container.Resolve<Consumes<RequestSeparateDocuments>.All>() });
			//_requestClassifyDocumentsCommandHandler = _container.Resolve<Consumes<RequestClassifyDocuments>.All>();
			//_requestSeparateDocumentsCommandHandler = _container.Resolve<Consumes<RequestSeparateDocuments>.All>();
		}

		[Then]
		public void corresponding_implemenations_should_be_correct()
		{
			_eventStore.ShouldBeOfType<InMemoryEventStore>();
			_eventPublisher.ShouldBeOfType<EventPublisher>();
			_repository.ShouldBeOfType<SimpleEventRepository>();
			//IEnumerable<Type> cmdHandlerTypes = _commandHandlers.Select(cmdHand => cmdHand.GetType());
			//cmdHandlerTypes.ShouldContain(new[]
			//                                        {
			//                                            typeof (RequestClassifyDocumentsCommandHandler),
			//                                            typeof (RequestSeparateDocumentsCommandHandler)
			//                                        });
			//_requestClassifyDocumentsCommandHandler.ShouldBeOfType<RequestClassifyDocumentsCommandHandler>();
			//_requestSeparateDocumentsCommandHandler.ShouldBeOfType<RequestSeparateDocumentsCommandHandler>();
		}

		[TestFixtureTearDown]
		public void Close_container()
		{
			_container.Dispose();
		}
	}
}
