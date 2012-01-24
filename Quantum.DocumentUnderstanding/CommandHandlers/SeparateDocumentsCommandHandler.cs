using MassTransit;
using Quantum.Commands;

namespace Quantum.DocumentUnderstanding.CommandHandlers
{
	public class SeparateDocumentsCommandHandler : Consumes<SeparateDocuments>.All
	{
		public void Consume(SeparateDocuments command)
		{

		}
	}
}