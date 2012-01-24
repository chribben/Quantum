using System;
using MassTransit;

namespace Quantum.Domain.Events
{
	public class SeparateDocumentsRequested : DomainEvent, CorrelatedBy<Guid>
	{
		public SeparateDocumentsRequested(Guid aggregateId, int aggregateVersion) : base(aggregateId, aggregateVersion)
		{
		}

		public Guid CorrelationId
		{
			get { return AggregateId; }
		}

		public FlowState State
		{
			get { return FlowState.SeparationRequested; }
		}
	}
}