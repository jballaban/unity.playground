using System;
using System.Collections.Generic;

namespace API.Memory.Internal
{

    public struct MemoryID
    {
        KeyValuePair<Type, ValueType> value;

        public MemoryID(Type type, ValueType id)
        {
            this.value = new KeyValuePair<Type, ValueType>(type, id);
        }
    }
}