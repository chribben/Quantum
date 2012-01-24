using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonDomain.Persistence;
using MassTransit;
using Quantum.Commands;

namespace Quantum.Domain.CommandHandlers
{
	public abstract class CommandHandlerBase<T> : Consumes<T>.All where T:class
	{
		private readonly IRepository _repository;

		protected CommandHandlerBase(IRepository repository)
		{
			_repository = repository;
		}

		public IRepository Repository
		{
			get { return _repository; }
		}

		public abstract void Consume(T message);
	}

	public interface ICommandHandler<out T>
	{
	}
}
