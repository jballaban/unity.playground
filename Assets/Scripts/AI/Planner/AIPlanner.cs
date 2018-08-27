using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class AIPlanner
{
    AIPlannerIAgent agent;
    public AIPlanner(AIPlannerIAgent agent)
    {
        this.agent = agent;
    }

    public Queue<AIPlannerIAction> Plan(HashSet<AIPlannerIAction> actions, AIPlannerIState worldstate, AIPlannerIState goalstate)
    {
        foreach (var a in actions)
            a.Reset();
        var usableActions = new HashSet<AIPlannerIAction>(actions); // not checking for valid yet
        var valids = new List<string>();
        foreach (var action in usableActions)
        {
            if (worldstate.Test(action.preConditions))
            {
                valids.Add(action.GetType().Name);
            }
        }
        Debugger.Instance.Log<AIPlanner>("Valid Next Actions: " + String.Join(", ", valids.ToArray()), agent.gameObject);
        var leaves = new List<AIPlannerNode>();
        var root = new AIPlannerNode(null, 0f, worldstate, null);
        if (!BuildGraph(root, ref leaves, usableActions, goalstate))
            return null; // no valid plans found
        var cheapest = leaves.OrderBy(l => l.runningCost).First();
        var ancestors = new List<AIPlannerIAction>();
        while (cheapest != null)
        {
            if (cheapest.action != null)
                ancestors.Insert(0, cheapest.action);
            cheapest = cheapest.parent;
        }
        Debugger.Instance.Log<AIPlanner>("New Plan: " + String.Join(", ", ancestors.Select(a => a.GetType().Name).ToArray()), agent.gameObject);
        return new Queue<AIPlannerIAction>(ancestors);
    }

    bool BuildGraph(AIPlannerNode parent, ref List<AIPlannerNode> leaves, HashSet<AIPlannerIAction> actions, AIPlannerIState goalstate)
    {
        bool result = false;
        foreach (var action in actions)
        {
            if (parent.state.Test(action.preConditions))
            {
                var state = (AIPlannerIState)parent.state.Clone();
                state.Apply(action.effects);
                var node = new AIPlannerNode(parent, parent.runningCost + action.cost, state, action);
                if (state.Test(goalstate))
                {
                    leaves.Add(node);
                    result = true;
                }
                else
                {
                    var subset = new HashSet<AIPlannerIAction>(actions.Where(a => !a.Equals(action))); // remove this action from the list and look again
                    result = BuildGraph(node, ref leaves, subset, goalstate) || result;
                }
            }
        }
        return result;
    }
}