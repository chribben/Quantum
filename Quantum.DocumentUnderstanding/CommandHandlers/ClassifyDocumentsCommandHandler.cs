using MassTransit;
using Quantum.Commands;

namespace Quantum.DocumentUnderstanding.CommandHandlers
{
	public class ClassifyDocumentsCommandHandler : Consumes<ClassifyDocuments>.All
	{
		public void Consume(ClassifyDocuments command)
		{

		}
	}
}