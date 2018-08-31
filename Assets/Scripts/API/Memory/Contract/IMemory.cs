using System;
using System.Collections.Generic;

namespace API.Memory.Contract
{
	public interface IMemory
	{
		IMemoryID id { get; }
	}
}