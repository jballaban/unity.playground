using System;

namespace API.AI.Memory.Contract
{
    public struct MemoryID<T> : IMemoryID where T : IMemory
    {
        ValueType value;
        int computedHashCode;

        public MemoryID(ValueType value)
        {
            this.value = value;
            this.computedHashCode = typeof(T).GetHashCode() ^ value.GetHashCode();
        }

        public override bool Equals(Object obj)
        {
            return obj is MemoryID<T> && this == (MemoryID<T>)obj;
        }

        public override int GetHashCode()
        {
            return computedHashCode;
        }

        public static bool operator ==(MemoryID<T> x, MemoryID<T> y)
        {
            return Object.Equals(x.value, y.value);
        }

        public static bool operator !=(MemoryID<T> x, MemoryID<T> y)
        {
            return !(x == y);
        }

        public override string ToString()
        {
            return typeof(T).Name + ":" + value?.ToString();
        }
    }
}