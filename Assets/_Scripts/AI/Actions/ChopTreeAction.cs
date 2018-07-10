public class ChopTreeAction : ExtractAction<TreeResource, LogResource>
{
    public override ResourceManager OutputManager
    {
        get
        {
            return ManagerBase.Instance.GetManager<LogManager>();
        }
    }

}