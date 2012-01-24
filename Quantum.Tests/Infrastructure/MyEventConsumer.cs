using System.Threading;
using MassTransit;
using Quantum.Domain.Events;

namespace Quantum.Tests.Infrastructure
{
	public class MyEventConsumer : Consumes<SeparateDocumentsRequested>.All
	{
		private readonly ManualResetEvent _received;

		public MyEventConsumer(ManualResetEvent received)
		{
			_received = received;
		}

		public void Consume(SeparateDocumentsRequested message)
		{
			_received.Set();
		}
	}
}