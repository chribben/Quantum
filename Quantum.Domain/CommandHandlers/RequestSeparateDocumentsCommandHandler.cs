using System;
using CommonDomain.Persistence;
using Magnum;
using MassTransit;
using Quantum.Commands;
using Quantum.Domain.Domain;

namespace Quantum.Domain.CommandHandlers
{
	public class RequestSeparateDocumentsCommandHandler : CommandHandlerBase<RequestSeparateDocuments>
	{
		public RequestSeparateDocumentsCommandHandler(IRepository repository) : base(repository)
		{
		}

		public override void Consume(RequestSeparateDocuments message)
		{
			var batch = Repository.GetById<Batch>(message.ArId);
			batch.RequestDocumentSeparation();
			Repository.Save(batch, Guid.NewGuid(), null);
		}
	}
}