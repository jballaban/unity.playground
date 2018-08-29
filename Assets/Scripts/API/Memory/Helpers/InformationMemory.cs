using System;
using System.Collections.Generic;
using API.Memory.Contract;

public class InformationMemory : IMemory
{
	public KeyValuePair<Type, ValueType> id { get; set; }
	public string topic { get; set; }
	public object data { get; set; }

	public InformationMemory(string topic, object data)
	{
		this.topic = topic;
		this.data = data;
		this.id = new KeyValuePair<Type, ValueType>(this.GetType(), topic.GetHashCode());
	}
}