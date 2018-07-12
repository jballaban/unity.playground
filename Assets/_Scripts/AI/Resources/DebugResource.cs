using System.Collections.Generic;
using UnityEngine;

public class DebugResource : ResourceBase
{
    public override IResourceManager Manager { get { return ManagerBase.Instance.GetManager<DebugManager>(); } }
}