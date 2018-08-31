using System;
using System.Collections.Generic;
using API.Memory.Contract;
using API.Memory.Internal;

public class InformationMemory<T> : IMemory
{
    public MemoryID id { get; set; }
    public string topic { get; set; }
    public T data { get; set; }

    public InformationMemory(string topic, T data)
    {
        this.topic = topic;
        this.data = data;
        this.id = new MemoryID(this.GetType(), topic.GetHashCode());
    }
}