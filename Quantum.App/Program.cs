using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Quantum.Commands;
using Quantum.Infrastructure.Installers;
using Magnum;
using MassTransit;
using log4net;
using log4net.Config;

namespace Quantum.App
{
	internal class Program
	{
		private static readonly ILog _Logger = LogManager.GetLogger(typeof (Program));
		
		private IWindsorContainer _Container;

		private static void Main()
		{
			Description();
			XmlConfigurator.Configure();

			var p = new Program();
			try { p.Start(); }
			finally { p.Stop(); }
		}

		private void Start()
		{
			try
			{
				_Logger.Info("installing and setting up components");
				_Container = new WindsorContainer()
					.Install(new BusInstaller("rabbitmq://localhost/Quantum.App"));

				_Container.Register(Component.For<IWindsorContainer>().Instance(_Container));
				while (true)
				{
					var batchId = CombGuid.Generate();

					Console.WriteLine("Press a key to create a batch.");
					Console.ReadKey(true);
					CreateBatch(batchId);
					Console.WriteLine("Press a key to send batch to document understanding.");
					Console.ReadKey(true);
					SendBatchToDocumentUnderstanding(batchId);

				}
			}
			catch (Exception ex)
			{
				_Logger.Error("exception thrown", ex);
			}

			Console.WriteLine("Press any key to finish.");
			Console.ReadKey(true);
		}

		private void SendBatchToDocumentUnderstanding(Guid batchId)
		{
			GetDomainService().Send(new RequestSeparateDocuments(batchId));
		}

		private void CreateBatch(Guid batchId)
		{
			GetDomainService().Send(new CreateBatch(batchId));
		}

		private static void Description()
		{
			Console.WriteLine(@"This application:
* Creates batches
* Send batches for document separation
* Send batches for classification.");
		}


		private void CreateCustomer(Guid aggregateId)
		{
			//GetDomainService()
			//    .Send(new CreateNewCustomer(aggregateId, "Jörg Egretzberger", "Meine Straße", "1", "1010", "Wien", "01/123456"));
		}

		private void RelocateCustomer(Guid customerId)
		{
			//GetDomainService()
			//    .Send(new RelocateTheCustomer(customerId, "Messestraße", "2", "4444", "Linz"));
		}

		private IEndpoint GetDomainService()
		{
			var bus = _Container.Resolve<IServiceBus>();
			var domainService = bus.GetEndpoint(new Uri("rabbitmq://localhost/Quantum.Domain.Service"));
			return domainService;
		}

		private void Stop()
		{
			_Container.Dispose();
		}
	}
}