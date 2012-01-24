using System;
using CommonDomain;
using CommonDomain.Core;
using Quantum.Domain.Events;
using Magnum;

namespace Quantum.Domain.Domain
{
	public class Batch : AggregateBase
	{
		private FlowState _state;

		public Batch()
		{
			
		}
		public Batch(Guid id)
		{
			RaiseEvent(new BatchCreated(id));			
		}

		public void Apply(BatchCreated @event)
		{
			 Id = @event.AggregateId;
		}

		public void Apply(ClassifyDocumentsRequested @event)
		{
			_state = @event.State;
		}

		public void Apply(SeparateDocumentsRequested @event)
		{
			_state = @event.State;
		}

		public void RequestDocumentClassification()
		{
			RaiseEvent(new ClassifyDocumentsRequested ( Id, 1));
		}

		public void RequestDocumentSeparation()
		{
			RaiseEvent(new SeparateDocumentsRequested(Id, 1));
		}
	}
}