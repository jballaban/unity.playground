public class DebugMemory : MemoryBase
{
    public override Recollection Remember(ResourceBase fact)
    {
        var recollection = base.Remember(fact);
        if (recollection.Status == Recollection.Statuses.New && recollection is ResourceBaseRecollection)
        {
            var resource = recollection as ResourceBaseRecollection;
            ManagerBase.Instance.GetManager<DebugManager>().Create(resource.Position, resource.Quantity, resource.Manager);
        }
        return recollection;
    }
}