using System.Collections.Generic;
using UnityEngine;

public class TreeResource : ResourceBase
{
    public override ResourceManager Manager { get { return ManagerBase.Instance.GetManager<TreeManager>(); } }
}