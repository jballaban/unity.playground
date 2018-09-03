using System;
using System.Collections.Generic;

namespace API.AI.Memory.Contract
{
    public interface IMemory
    {
        IMemoryID id { get; }
    }
}