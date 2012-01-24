using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CommonDomain.Persistence;
using Quantum.EventStore;
using Quantum.Repository;

namespace Quantum.Infrastructure.Installers
{
	public class RepositoryInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Component.For<IConstructAggregates>().ImplementedBy<AggregateFactory>());
			container.Register(Component.For<IRepository>().ImplementedBy<SimpleEventRepository>().LifeStyle.Singleton);
		}
	}
}