using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NavigationBase))]
public abstract class AgentBase : MonoBehaviour
{
	public HashSet<ActionBase> AvailableActions;
	public Queue<ActionBase> CurrentActions;
	public Planner Planner;
	public FSMState IdleState;
	public FSMState MoveState;
	public FSMState ActState;
	public FSM StateMachine;
	public State WorldState;
	public State GoalState;
	public Backpack Backpack;
	public Memory Memory;
	public NavigationBase Navigation;

	protected virtual void Start()
	{
		Planner = new Planner();
		CurrentActions = new Queue<ActionBase>();
		AvailableActions = new HashSet<ActionBase>();
		WorldState = new State();
		Backpack = new Backpack(WorldState);
		Memory = new Memory(WorldState);
		GoalState = new State();
		foreach (var a in gameObject.GetComponents<ActionBase>())
			AvailableActions.Add(a);
		IdleState = new IdleState(this);
		MoveState = new MoveState(this);
		ActState = new ActState(this);
		StateMachine = new FSM();
		StateMachine.PushState(IdleState);
		Navigation = gameObject.GetComponent<NavigationBase>();
	}

	protected virtual void Update()
	{
		StateMachine.Update();
	}

	private void OnTriggerEnter(Collider other)
	{
		var resource = other.GetComponent<ResourceBase>();
		if (resource != null)
			Memory.Remember(resource);
	}
}