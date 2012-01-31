using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CommonDomain.Persistence;
using MassTransit;
using MassTransit.Saga;
using Quantum.Commands;
using Quantum.Domain.CommandHandlers;
using Quantum.Domain.Events;

namespace Quantum.Infrastructure.Installers
{
	public class CommandHandlerInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			//container.AddFacility<TypedFactoryFacility>();


			//container.Register(
			//    AllTypes.FromAssemblyContaining(typeof(RequestClassifyDocumentsCommandHandler))
			//    .Where(x => x. GetInterface(typeof(Consumes<>.All).Name) != null && !x.IsSubclassOf(typeof(ISaga))));
			//container.Register(Component.For<ICommandHandler<RequestClassifyDocuments>>().ImplementedBy<RequestClassifyDocumentsCommandHandler>());
			//container.Register(Component.For<ICommandHandler<RequestSeparateDocuments>>().ImplementedBy<RequestSeparateDocumentsCommandHandler>());
			//container.Register(Component.For<SeparateDocumentsEventHandler>().LifeStyle.Singleton);
			container.Register(Component.For<CreateBatchCommandHandler>().LifeStyle.Singleton);
			container.Register(Component.For<RequestClassifyDocumentsCommandHandler>().LifeStyle.Singleton);
			container.Register(Component.For<RequestSeparateDocumentsCommandHandler>().LifeStyle.Singleton);
		}
	}

	//public class SeparateDocumentsEventHandler : CommandHandlerBase<SeparateDocumentsRequested>
	//{
	//    public SeparateDocumentsEventHandler(IRepository repo) : base(repo)
	//    {
			
	//    }
	//    public override void Consume(SeparateDocumentsRequested message)
	//    {
	//        throw new System.NotImplementedException();
	//    }
	//}
}