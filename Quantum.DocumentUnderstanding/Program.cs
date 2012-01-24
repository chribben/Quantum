using System.Threading;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Quantum.Infrastructure.Installers;
using MassTransit;
using Topshelf;
using log4net;
using log4net.Config;

namespace Quantum.DocumentUnderstanding
{
	class Program
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

		private IWindsorContainer _container;
		private IServiceBus _bus;

		public static void Main(string[] args)
		{
			Thread.CurrentThread.Name = "Document understanding service Main Thread";
			HostFactory.Run(x =>
			{
				x.Service<Program>(s =>
				{
					s.ConstructUsing(name => new Program());
					s.WhenStarted(p => p.Start());
					s.WhenStopped(p => p.Stop());
				});
				x.RunAsLocalSystem();

				x.SetDescription("Handles the document understanding for the Quantum Application.");
				x.SetDisplayName("Quantum Document Understanding Service");
				x.SetServiceName("Quantum.DocumentUnderstandingService");
			});
		}

		private void Start()
		{
			XmlConfigurator.Configure();
			Logger.Info("setting up domain service, installing components");

			_container = new WindsorContainer().Install(new BusInstaller("rabbitmq://localhost/Quantum.DocumentUnderstandingService"));
			_container.Register(AllTypes.FromThisAssembly().BasedOn(typeof(Consumes<>.All)));
			_container.Register(Component.For<IWindsorContainer>().Instance(_container));
			_bus = _container.Resolve<IServiceBus>();

			Logger.Info("application configured, started running");
		}

		private void Stop()
		{
			Logger.Info("shutting down Domain Service");
			_container.Release(_bus);
			_container.Dispose();
		}
	}
}
