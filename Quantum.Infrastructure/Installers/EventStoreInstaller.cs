using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Quantum.EventStore;

namespace Quantum.Infrastructure.Installers
{
	public class EventStoreInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Component.For<IEventStore>().ImplementedBy<InMemoryEventStore>().LifeStyle.Singleton);
		}
	}
}