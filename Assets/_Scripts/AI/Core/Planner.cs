using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Planner
{
	public Queue<ActionBase> Plan(HashSet<ActionBase> actions, State worldstate, State goalstate)
	{
		foreach (var a in actions)
			a.Reset();
		var usableActions = new HashSet<ActionBase>(actions); // not checking for valid yet
		var valids = new List<string>();
		foreach (var action in usableActions)
		{
			if (worldstate.Test(action.PreConditions))
			{
				valids.Add(action.GetType().Name);
			}
		}
		Debug.Log("Valid Next Actions: " + String.Join(", ", valids.ToArray()));
		var leaves = new List<Node>();
		var root = new Node(null, 0f, worldstate, null);
		if (!BuildGraph(root, ref leaves, usableActions, goalstate))
			return null; // no valid plans found
		var cheapest = leaves.OrderBy(l => l.RunningCost).First();
		var ancestors = new List<ActionBase>();
		while (cheapest != null)
		{
			if (cheapest.Action != null)
				ancestors.Insert(0, cheapest.Action);
			cheapest = cheapest.Parent;
		}
		return new Queue<ActionBase>(ancestors);
	}

	bool BuildGraph(Node parent, ref List<Node> leaves, HashSet<ActionBase> actions, State goalstate)
	{
		bool result = false;
		foreach (var action in actions)
		{
			if (parent.State.Test(action.PreConditions))
			{
				var state = new State(parent.State);
				state.Apply(action.Effects);
				var node = new Node(parent, parent.RunningCost + action.Cost, state, action);
				if (state.Test(goalstate))
				{
					leaves.Add(node);
					result = true;
				}
				else
				{
					var subset = new HashSet<ActionBase>(actions.Where(a => !a.Equals(action))); // remove this action from the list and look again
					result = BuildGraph(node, ref leaves, subset, goalstate) || result;
				}
			}
		}
		return result;
	}

	class Node
	{
		public Node Parent;
		public float RunningCost;
		public State State;
		public ActionBase Action;

		public Node(Node parent, float runningCost, State state, ActionBase action)
		{
			this.Parent = parent;
			this.RunningCost = runningCost;
			this.State = state;
			this.Action = action;
		}
	}
}