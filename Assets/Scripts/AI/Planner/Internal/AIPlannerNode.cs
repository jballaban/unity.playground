internal class AIPlannerNode
{
    public AIPlannerNode parent;
    public float runningCost;
    public AIPlannerIState state;
    public AIPlannerIAction action;

    internal AIPlannerNode(AIPlannerNode parent, float runningCost, AIPlannerIState state, AIPlannerIAction action)
    {
        this.parent = parent;
        this.runningCost = runningCost;
        this.state = state;
        this.action = action;
    }
}