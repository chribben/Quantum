using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MassTransit;
using log4net.Config;

namespace Quantum.Tests.Infrastructure
{
	public class LocalBusInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
container.Register(
	Component.For<IServiceBus>()
		.UsingFactoryMethod(() => ServiceBusFactory.New(x =>
															{
																x.ReceiveFrom("loopback://localhost/mt_client");
																x.Subscribe(conf => conf.LoadFrom(container));
															})));

		}
	}
}