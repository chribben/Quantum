using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MassTransit;
using Quantum.Commands;
using Quantum.Domain.CommandHandlers;

namespace Quantum.Infrastructure.Installers
{
	public class CommandHandlerInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.AddFacility<TypedFactoryFacility>();

			//container.Register(
			//    AllTypes.FromAssemblyContaining(typeof(RequestClassifyDocumentsCommandHandler))
			//    .Where(x => x.BaseType == typeof(ICommandHandler<>)));
			//container.Register(Component.For<ICommandHandler<RequestClassifyDocuments>>().ImplementedBy<RequestClassifyDocumentsCommandHandler>());
			//container.Register(Component.For<ICommandHandler<RequestSeparateDocuments>>().ImplementedBy<RequestSeparateDocumentsCommandHandler>());
			container.Register(Component.For<RequestClassifyDocumentsCommandHandler>().LifeStyle.Singleton);
			container.Register(Component.For<RequestSeparateDocumentsCommandHandler>().LifeStyle.Singleton);
		}
	}
}