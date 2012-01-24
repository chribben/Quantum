using System;
using MassTransit;

namespace Quantum.Domain.Events
{
	public class BatchCreated : DomainEvent, CorrelatedBy<Guid>
	{
		public BatchCreated(Guid aggregateId)
		{
			AggregateId = aggregateId;
		}

		public Guid CorrelationId
		{
			get { return AggregateId; }
		}
	}
}