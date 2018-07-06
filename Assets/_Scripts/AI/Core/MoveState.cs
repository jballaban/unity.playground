using UnityEngine;

public class MoveState : FSMState
{
    AgentBase _agent;
    public MoveState(AgentBase agent)
    {
        _agent = agent;
    }

    public void Update(FSM fsm, GameObject gameObject)
    {
        var action = _agent.CurrentActions.Peek();
        if (action.RequiresInRange && action.Target == null)
        {
            fsm.PopState(); // move
            fsm.PopState(); // perform
            fsm.PushState(_agent.IdleState);
            return;
        }

        /*   // get the agent to move itself
          if (dataProvider.moveAgent(action))
          {
              fsm.popState();
          } */

    }

}