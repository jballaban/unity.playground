using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveState : FSMState
{
    AgentBase _agent;
    NavMeshAgent _nav;
    public float RethinkDelay = 5f;
    public float SuccessDistance = 1f;
    float _rethinkTime;
    public MoveState(AgentBase agent)
    {
        _agent = agent;
        _nav = agent.gameObject.GetComponent<NavMeshAgent>();
    }

    public void Enter()
    {
        _rethinkTime = 0f;
        if (_agent.CurrentActions.Peek().Target == null)
            Debug.LogError("Should not Enter MoveState without an action Target");
        _nav.SetDestination(_agent.CurrentActions.Peek().Target.transform.position);
    }

    public void Exit()
    {
        _nav.isStopped = true;
    }

    public void Update(FSM fsm)
    {
        var action = _agent.CurrentActions.Peek();
        if (_agent.CurrentActions.Count == 0 || !_nav.hasPath || !action.RequiresInRange || action.Target == null)
        {
            fsm.PopState(); // move
            fsm.PopState(); // perform
            fsm.PushState(_agent.IdleState);
            return;
        }
        if (_nav.remainingDistance <= SuccessDistance)
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
        _nav.SetDestination(_agent.CurrentActions.Peek().Target.transform.position);
    }

}