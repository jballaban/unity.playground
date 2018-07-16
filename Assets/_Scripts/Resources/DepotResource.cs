public class DepotResource : ResourceBase
{
	public override IResourceManager Manager { get { return ManagerBase.Instance.GetManager<DepotManager>(); } }
}