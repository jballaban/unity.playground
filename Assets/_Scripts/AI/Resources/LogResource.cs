using System.Collections.Generic;
using UnityEngine;

public class LogResource : ResourceBase
{
    public override ResourceManager Manager { get { return ManagerBase.Instance.GetManager<LogManager>(); } }
}