using System;
using MassTransit;

namespace Quantum.Domain.Events
{
	public class ClassifyDocumentsRequested : DomainEvent, CorrelatedBy<Guid>
	{
		public ClassifyDocumentsRequested(Guid id, int version) : base(id, version)
		{
		}

		public FlowState State 
		{
			get { return FlowState.ClassifyRequested; }
		}

		public Guid CorrelationId
		{
			get { return AggregateId; }
		}
	}
}