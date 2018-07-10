public class DepotResource : ResourceBase
{
    public override ResourceManager Manager { get { return ManagerBase.Instance.GetManager<DepotManager>(); } }
}