public class ChopTreeAction : ExtractAction<TreeResource, LogResource>
{
    public override bool Perform(AgentBase agent)
    {
        agent.Backpack.Inc<LogResource>(1f);
        return true;
    }
}