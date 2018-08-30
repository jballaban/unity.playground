using System;
using System.Collections.Generic;
using API.Memory.Contract;

public class InformationMemory<T> : IMemory
{
    public KeyValuePair<Type, ValueType> id { get; set; }
    public string topic { get; set; }
    public T data { get; set; }

    public InformationMemory(string topic, T data)
    {
        this.topic = topic;
        this.data = data;
        this.id = new KeyValuePair<Type, ValueType>(this.GetType(), topic.GetHashCode());
    }
}