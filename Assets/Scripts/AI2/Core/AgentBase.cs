using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NavigationSystem))]
public abstract class AgentBase : MonoBehaviour, IAIAgent
{
    public HashSet<IAIAction> AvailableActions;
    public Queue<IAIAction> CurrentActions;
    public AIPlanner Planner;
    public FSMState IdleState;
    public FSMState MoveState;
    public FSMState ActState;
    public FSM StateMachine;
    public State WorldState;
    public State GoalState;
    public Backpack Backpack;
    public NavigationSystem Navigation;

    protected virtual void Awake()
    {
        Planner = new AIPlanner(this);
        CurrentActions = new Queue<IAIAction>();
        AvailableActions = new HashSet<IAIAction>();
        WorldState = new State();
        Backpack = new Backpack(WorldState);
        GoalState = new State();
        foreach (var a in gameObject.GetComponents<ActionBase>())
            AvailableActions.Add(a);
        IdleState = new IdleState(this);
        MoveState = new MoveState(this);
        ActState = new ActState(this);
        StateMachine = new FSM();
        StateMachine.PushState(IdleState);
        Navigation = gameObject.GetComponent<NavigationSystem>();
    }

    protected virtual void Update()
    {
        StateMachine.Update();
    }
}