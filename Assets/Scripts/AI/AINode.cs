internal class AINode
{
    public AINode parent;
    public float runningCost;
    public IAIState state;
    public IAIAction action;

    internal AINode(AINode parent, float runningCost, IAIState state, IAIAction action)
    {
        this.parent = parent;
        this.runningCost = runningCost;
        this.state = state;
        this.action = action;
    }
}