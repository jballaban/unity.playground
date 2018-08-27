using System;
using System.Linq;
using UnityEngine;

public class IdleState : FSMState
{
    AgentBase _agent;
    public float RethinkDelay = 10f;
    float _rethinkTime = 0f;
    public IdleState(AgentBase agent)
    {
        _agent = agent;
    }

    public void Enter()
    {
        _rethinkTime = RethinkDelay; // force an immediate rethink
    }

    public void Exit() { }

    public void Update(FSM fsm)
    {
        _rethinkTime += Time.deltaTime;
        if (_rethinkTime < RethinkDelay) return;
        _rethinkTime -= RethinkDelay;
        var plan = _agent.Planner.Plan(_agent.AvailableActions, _agent.WorldState, _agent.GoalState);
        if (plan != null)
        {
            //Debug.Log("Plan: " + String.Join(", ", plan.Select(s => s.GetType().Name).ToArray()));
            // we have a plan, hooray!
            _agent.CurrentActions = plan;
            //   dataProvider.planFound(goal, plan);
            fsm.PopState(); // move to PerformAction state
            fsm.PushState(_agent.ActState);
        }
        else
        {
            //Debug.Log("No valid plan");
        }

    }

}