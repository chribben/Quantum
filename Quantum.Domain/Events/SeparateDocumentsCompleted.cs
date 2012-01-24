using System;
using MassTransit;

namespace Quantum.Domain.Events
{
	public class SeparateDocumentsCompleted : DomainEvent, CorrelatedBy<Guid>
	{
		public SeparateDocumentsCompleted(Guid aggregateId, int aggregateVersion) : base(aggregateId, aggregateVersion)
		{
		}

		public Guid CorrelationId
		{
			get { return AggregateId; }
		}

		public FlowState State
		{
			get { return FlowState.SeparationCompleted; }
		}
	}
}