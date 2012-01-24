using System;

namespace Quantum.Commands
{
	public class RequestClassifyDocuments : Command
	{
		public RequestClassifyDocuments(Guid arId) : base(arId)
		{
		}
	}
}