using System;
using CommonDomain;
using CommonDomain.Persistence;
using Magnum;
using MassTransit;
using Quantum.Commands;
using Quantum.Domain.Domain;

namespace Quantum.Domain.CommandHandlers
{
	public class RequestClassifyDocumentsCommandHandler : CommandHandlerBase<RequestClassifyDocuments>
	{
		public RequestClassifyDocumentsCommandHandler(IRepository repository) : base(repository)
		{
		}

		public override void Consume(RequestClassifyDocuments message)
		{
			var batch = Repository.GetById<Batch>(message.ArId);
			batch.RequestDocumentClassification();
			Repository.Save(batch, Guid.NewGuid(), null);
		}
	}
}