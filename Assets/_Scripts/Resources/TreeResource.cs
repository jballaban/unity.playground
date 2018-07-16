using System.Collections.Generic;
using UnityEngine;

public class TreeResource : ResourceBase
{
	public override IResourceManager Manager { get { return ManagerBase.Instance.GetManager<TreeManager>(); } }
}