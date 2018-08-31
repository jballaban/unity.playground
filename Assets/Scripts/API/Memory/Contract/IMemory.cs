using System;
using System.Collections.Generic;
using API.Memory.Internal;

namespace API.Memory.Contract
{
    public interface IMemory
    {
        MemoryID id { get; }
    }
}