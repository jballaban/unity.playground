using UnityEngine;

public class ActState : FSMState
{
    AgentBase _agent;
    public ActState(AgentBase agent)
    {
        _agent = agent;
    }

    public void Enter() { }
    public void Exit() { }

    public void Update(FSM fsm)
    {
        // perform the action
        if (_agent.CurrentActions.Count == 0)
        {
            // no actions to perform
            fsm.PopState();
            fsm.PushState(_agent.IdleState);
            // dataProvider.actionsFinished();
            return;
        }

        var action = _agent.CurrentActions.Peek() as AIActionBase;
        if (action.isDone)
            _agent.CurrentActions.Dequeue();

        if (_agent.CurrentActions.Count > 0)
        {
            // perform the next action
            action = _agent.CurrentActions.Peek() as AIActionBase;
            if (!(action is ProximityActionBase) || (action is ProximityActionBase && (action as ProximityActionBase).IsInRange))
            {
                // we are in range, so perform the action
                if (!action.Perform(_agent))
                {
                    // action failed, we need to plan again
                    fsm.PopState();
                    fsm.PushState(_agent.IdleState);
                    //dataProvider.planAborted(action);
                }
            }
            else
            {
                // we need to move there first
                // push moveTo state
                (action as ProximityActionBase).DetermineTarget(_agent);
                fsm.PushState(_agent.MoveState);
            }
        }
        else
        {
            // no actions left, move to Plan state
            fsm.PopState();
            fsm.PushState(_agent.IdleState);
            //  dataProvider.actionsFinished();
        }
    }
}