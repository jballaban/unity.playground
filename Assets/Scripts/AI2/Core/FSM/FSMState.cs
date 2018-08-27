using UnityEngine;

public interface FSMState
{

    void Update(FSM fsm);
    void Enter();
    void Exit();
}