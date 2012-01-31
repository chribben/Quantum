using System;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MassTransit;
using MassTransit.Saga;
using Quantum.Domain.CommandHandlers;
using Quantum.Domain.Domain;

namespace Quantum.Infrastructure.Installers
{
	public class SagaInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
				Component.For<ISaga>().ImplementedBy<DocumentUnderstandingSaga>(),
				Component.For(typeof(ISagaRepository<>))
					.ImplementedBy(typeof(InMemorySagaRepository<>))
					.LifeStyle.Singleton);


			//container.Register(Component.For<ISagaRepository<DocumentUnderstandingSaga>>().
			//    ImplementedBy<InMemorySagaRepository<DocumentUnderstandingSaga>>().
			//    LifeStyle.Singleton);
			//container.Register(
			//    AllTypes.FromAssemblyContaining(typeof(DocumentUnderstandingSaga))
			//    .Where(x => x.GetInterface(typeof(ISaga).Name) != null));
			//container.Register(Component.For<ISaga>().ImplementedBy<DocumentUnderstandingSaga>());
		}
	}
}