using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MassTransit;
using Quantum.EventStore;

namespace Quantum.Infrastructure.Installers
{
	public class EventPublisherInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Component.For<IEventPublisher>()
							.UsingFactoryMethod((k, c) => 
								new EventPublisher(k.Resolve<IServiceBus>())).LifeStyle.Singleton);
		}
	}
}