using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NavigationSystem))]
public abstract class AgentBase : MonoBehaviour, AIPlannerIAgent, AIActionIAgent
{
    public HashSet<AIPlannerIAction> AvailableActions;
    public Queue<AIPlannerIAction> CurrentActions;
    public AIPlanner Planner;
    public FSMState IdleState;
    public FSMState MoveState;
    public FSMState ActState;
    public FSM StateMachine;
    public State WorldState;
    public State GoalState;
    public Backpack Backpack;
    public NavigationSystem Navigation { get; set; }

    protected virtual void Awake()
    {
        Planner = new AIPlanner(this);
        CurrentActions = new Queue<AIPlannerIAction>();
        AvailableActions = new HashSet<AIPlannerIAction>();
        WorldState = new State();
        Backpack = new Backpack(WorldState);
        GoalState = new State();
        foreach (var a in gameObject.GetComponents<AIActionBase>())
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