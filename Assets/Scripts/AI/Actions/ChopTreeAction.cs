public class ChopTreeAction : ExtractAction<TreeResource, LogResource>
{
	public override IResourceManager OutputManager
	{
		get
		{
			return ManagerBase.Instance.GetManager<LogManager>();
		}
	}

}