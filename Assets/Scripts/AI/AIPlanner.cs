using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class AIPlanner
{
    IAIAgent agent;
    public AIPlanner(IAIAgent agent)
    {
        this.agent = agent;
    }

    public Queue<IAIAction> Plan(HashSet<IAIAction> actions, IAIState worldstate, IAIState goalstate)
    {
        foreach (var a in actions)
            a.Reset();
        var usableActions = new HashSet<IAIAction>(actions); // not checking for valid yet
        var valids = new List<string>();
        foreach (var action in usableActions)
        {
            if (worldstate.Test(action.preConditions))
            {
                valids.Add(action.GetType().Name);
            }
        }
        Debugger.Instance.Log<AIPlanner>("Valid Next Actions: " + String.Join(", ", valids.ToArray()), agent.gameObject);
        var leaves = new List<AINode>();
        var root = new AINode(null, 0f, worldstate, null);
        if (!BuildGraph(root, ref leaves, usableActions, goalstate))
            return null; // no valid plans found
        var cheapest = leaves.OrderBy(l => l.runningCost).First();
        var ancestors = new List<IAIAction>();
        while (cheapest != null)
        {
            if (cheapest.action != null)
                ancestors.Insert(0, cheapest.action);
            cheapest = cheapest.parent;
        }
        Debugger.Instance.Log<AIPlanner>("New Plan: " + String.Join(", ", ancestors.Select(a => a.GetType().Name).ToArray()), agent.gameObject);
        return new Queue<IAIAction>(ancestors);
    }

    bool BuildGraph(AINode parent, ref List<AINode> leaves, HashSet<IAIAction> actions, IAIState goalstate)
    {
        bool result = false;
        foreach (var action in actions)
        {
            if (parent.state.Test(action.preConditions))
            {
                var state = (IAIState)parent.state.Clone();
                state.Apply(action.effects);
                var node = new AINode(parent, parent.runningCost + action.cost, state, action);
                if (state.Test(goalstate))
                {
                    leaves.Add(node);
                    result = true;
                }
                else
                {
                    var subset = new HashSet<IAIAction>(actions.Where(a => !a.Equals(action))); // remove this action from the list and look again
                    result = BuildGraph(node, ref leaves, subset, goalstate) || result;
                }
            }
        }
        return result;
    }
}