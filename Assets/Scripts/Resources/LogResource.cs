using System.Collections.Generic;
using UnityEngine;

public class LogResource : ResourceBase
{
	public override IResourceManager Manager { get { return ManagerBase.Instance.GetManager<LogManager>(); } }
}