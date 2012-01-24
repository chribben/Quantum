using System;
using CommonDomain.Persistence;
using Magnum;
using MassTransit;
using Quantum.Commands;
using Quantum.Domain.Domain;

namespace Quantum.Domain.CommandHandlers
{
	public class CreateBatchCommandHandler : CommandHandlerBase<CreateBatch>
	{
		public CreateBatchCommandHandler(IRepository repository) : base(repository)
		{
		}

		public override void Consume(CreateBatch message)
		{
			var batch = new Batch(message.ArId);
			Repository.Save(batch, Guid.NewGuid(), null);
		}
	}
}