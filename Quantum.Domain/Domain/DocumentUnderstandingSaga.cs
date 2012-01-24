using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MassTransit;
using MassTransit.Saga;
using Quantum.Commands;
using Quantum.Domain.Events;

namespace Quantum.Domain.Domain
{
	public class DocumentUnderstandingSaga:
		 InitiatedBy<SeparateDocumentsRequested>,
		 Orchestrates<SeparateDocumentsCompleted>,
		 Orchestrates<ClassifyDocumentsRequested>,
		 Orchestrates<ClassifyDocumentsCompleted>,
		 ISaga
	{
		public DocumentUnderstandingSaga(Guid correlationId)
		{
			CorrelationId = correlationId;
		}
		public void Consume(SeparateDocumentsRequested message)
		{
			//Command DocumentSeparation
		}
		public void Consume(SeparateDocumentsCompleted message)
		{

		}

		public void Consume(ClassifyDocumentsRequested message)
		{
		}

		public void Consume(ClassifyDocumentsCompleted message)
		{
		}

		public IServiceBus Bus { get; set; }

		public Guid CorrelationId { get; protected set; }
	}
}

