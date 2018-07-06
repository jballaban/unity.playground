using UnityEngine;

public class IdleState : FSMState
{
    AgentBase _agent;
    public IdleState(AgentBase agent)
    {
        _agent = agent;
    }

    public void Update(FSM fsm, GameObject gameObject)
    {
        var plan = _agent.Planner.Plan(gameObject, _agent.AvailableActions, _agent.WorldState, null);
        if (plan != null)
        {
            // we have a plan, hooray!
            _agent.CurrentActions = plan;
            //   dataProvider.planFound(goal, plan);
            fsm.PopState(); // move to PerformAction state
            fsm.PushState(_agent.ActionState);
        }
        else
        {
            // ugh, we couldn't get a plan
            // dataProvider.planFailed(goal);
            fsm.PopState(); // move back to IdleAction state
            fsm.PushState(_agent.IdleState);
        }
    }

}