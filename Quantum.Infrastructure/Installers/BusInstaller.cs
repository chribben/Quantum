using System.Linq;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MassTransit;
using MassTransit.Saga;
using Quantum.Domain.Domain;

namespace Quantum.Infrastructure.Installers
{
	/// <summary>
	/// Installs the service bus into the container.
	/// </summary>
	public class BusInstaller : IWindsorInstaller
	{
		private readonly string _EndpointUri;
		
		public BusInstaller(string endpointUri)
		{
			_EndpointUri = endpointUri;
		}

		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			// in proc bus
			//var bus = new InProcessBus(container);
			//container.Register(Component.For<IBus>().Instance(bus));

			// for factory
			if (!container.Kernel.GetFacilities()
				.Any(x => x.GetType().Equals(typeof(TypedFactoryFacility))))
				container.AddFacility<TypedFactoryFacility>();

			// masstransit bus
			//var busAdapter = new MassTransitPublisher(bus);

			container.Register(
				Component.For<IServiceBus>()
					.UsingFactoryMethod(() => ServiceBusFactory.New(sbc =>
					                                                	{
					                                                		sbc.UseRabbitMq();
					                                                		sbc.ReceiveFrom(_EndpointUri);
					                                                		sbc.UseRabbitMqRouting();
					                                                		sbc.Subscribe(c => c.LoadFrom(container));
					                                                	})).LifeStyle.Singleton);

            var bus = container.Resolve<IServiceBus>();

			bus.SubscribeSaga(new InMemorySagaRepository<DocumentUnderstandingSaga>());
			//Component.For<IBus>()
			//    .UsingFactoryMethod((k, c) => 
			//        new MassTransitPublisher(k.Resolve<IServiceBus>()))
			//    .Forward<IDispatchCommits>()
			//    .LifeStyle.Singleton);
		}
	}
}