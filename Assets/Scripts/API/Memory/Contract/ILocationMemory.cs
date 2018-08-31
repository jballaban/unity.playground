using UnityEngine;

namespace API.Memory.Contract
{
	public interface ILocationMemory : IMemory
	{
		bool Intersects(Vector3 center, float radius);
	}
}