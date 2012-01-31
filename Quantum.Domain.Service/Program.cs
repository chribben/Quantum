using System.Collections.Generic;
using System.Threading;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using MassTransit.Pipeline;
using MassTransit.Saga;
using MassTransit.Testing;
using Quantum.Commands;
using Quantum.Domain.Domain;
using Quantum.Domain.Events;
using Quantum.Infrastructure;
using Quantum.Infrastructure.Installers;
using MassTransit;
using Topshelf;
using log4net;
using log4net.Config;

namespace Quantum.Domain.Service
{
	class Program
	{
		private static readonly ILog _Logger = LogManager.GetLogger(typeof(Program));

		private IWindsorContainer _Container;
		private IServiceBus _Bus;

		public static void Main(string[] args)
		{
			Thread.CurrentThread.Name = "Domain Service Main Thread";
			HostFactory.Run(x =>
			{
				x.Service<Program>(s =>
				{
					s.ConstructUsing(name => new Program());
					s.WhenStarted(p => p.Start());
					s.WhenStopped(p => p.Stop());
				});
				x.RunAsLocalSystem();

				x.SetDescription("Handles the domain logic for the Quantum Application.");
				x.SetDisplayName("Quantum Domain Service");
				x.SetServiceName("Quantum.Domain.Service");
			});
		}

		private void Start()
		{
			XmlConfigurator.Configure();
			_Logger.Info("setting up domain service, installing components");

			_Container = new WindsorContainer().Install(
				new RepositoryInstaller(),
				new SagaInstaller(),
				new CommandHandlerInstaller(),
				new EventStoreInstaller(),
				new BusInstaller("rabbitmq://localhost/Quantum.Domain.Service"),
				new EventPublisherInstaller());

			_Container.Register(Component.For<IWindsorContainer>().Instance(_Container));
			_Bus = _Container.Resolve<IServiceBus>();
			_Logger.Info("application configured, started running");
		}

		private void Stop()
		{
			_Logger.Info("shutting down Domain Service");
			_Container.Release(_Bus);
			_Container.Dispose();
		}
	}
}
