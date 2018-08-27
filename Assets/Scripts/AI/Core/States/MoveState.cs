using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveState : FSMState
{
    AgentBase _agent;
    public float RethinkDelay = 5f;
    public float SuccessDistance = 1f;
    float _rethinkTime;
    NavigationSystem _nav;
    public MoveState(AgentBase agent)
    {
        _agent = agent;
        _nav = agent.gameObject.GetComponent<NavigationSystem>();
    }

    public void Enter()
    {
        _rethinkTime = 0f;
        _nav.GoTo((_agent.CurrentActions.Peek() as ProximityActionBase).Destination);
    }

    public void Exit()
    {
        _nav.Stop();
    }

    public void Update(FSM fsm)
    {
        ProximityActionBase action = null;
        if (_agent.CurrentActions.Count > 0)
            action = _agent.CurrentActions.Peek() as ProximityActionBase;
        if (action == null || !_nav.Valid() || action.Destination == Vector3.zero)
        {
            fsm.PopState(); // move
            fsm.PopState(); // perform
            fsm.PushState(_agent.IdleState);
            return;
        }
        if (_nav.RemainingDistance() <= SuccessDistance)
        {
            action.IsInRange = true;
            fsm.PopState();
            return;
        }
        _rethinkTime += Time.deltaTime;
        if (_rethinkTime > RethinkDelay)
        {
            _rethinkTime -= RethinkDelay;
            Rethink();
        }

        /*   // get the agent to move itself
          if (dataProvider.moveAgent(action))
          {
              fsm.popState();
          } */
    }

    void Rethink()
    {
        ProximityActionBase action = null;
        if (_agent.CurrentActions.Count > 0)
            action = _agent.CurrentActions.Peek() as ProximityActionBase;
        if (action != null)
            _nav.GoTo((_agent.CurrentActions.Peek() as ProximityActionBase).Destination);
    }

}