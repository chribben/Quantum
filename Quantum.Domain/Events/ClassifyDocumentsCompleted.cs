using System;
using MassTransit;

namespace Quantum.Domain.Events
{
	public class ClassifyDocumentsCompleted: DomainEvent, CorrelatedBy<Guid>
	{
		public ClassifyDocumentsCompleted(Guid id, int version) : base(id, version)
		{
		}

		public Guid CorrelationId
		{
			get { return AggregateId; }
		}

		public FlowState State
		{
			get { return FlowState.ClassifyCompleted; }
		}
	}
}