using System.Collections.Generic;
using UnityEngine;

public class AgentBase : MonoBehaviour
{
    public HashSet<ActionBase> AvailableActions;
    public Queue<ActionBase> CurrentActions;
    public Planner Planner;
    public FSMState IdleState;
    public FSMState MoveState;
    public FSMState ActionState;
    public FSM StateMachine;
    public State WorldState;

    void Start()
    {
        Planner = new Planner();
        CurrentActions = new Queue<ActionBase>();
        AvailableActions = new HashSet<ActionBase>();
        foreach (var a in gameObject.GetComponents<ActionBase>())
            AvailableActions.Add(a);
        IdleState = new IdleState(this);
        MoveState = new MoveState(this);
        ActionState = new ActionState(this);
        StateMachine.PushState(IdleState);
    }
}