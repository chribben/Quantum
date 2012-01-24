using System;

namespace Quantum.Commands
{
	public class RequestSeparateDocuments : Command
	{
		public RequestSeparateDocuments(Guid arId) : base(arId)
		{
		}
	}
}