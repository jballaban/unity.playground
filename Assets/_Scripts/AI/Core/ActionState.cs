using UnityEngine;

public class ActionState : FSMState
{
    AgentBase _agent;
    public ActionState(AgentBase agent)
    {
        _agent = agent;
    }

    public void Update(FSM fsm, GameObject gameObject)
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

        var action = _agent.CurrentActions.Peek();
        if (action.IsDone)
            _agent.CurrentActions.Dequeue();

        if (_agent.CurrentActions.Count > 0)
        {
            // perform the next action
            action = _agent.CurrentActions.Peek();
            bool inRange = action.RequiresInRange ? action.IsInRange : true;

            if (inRange)
            {
                // we are in range, so perform the action
                if (!action.Perform(gameObject))
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