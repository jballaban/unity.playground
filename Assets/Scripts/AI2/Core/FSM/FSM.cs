using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    Stack<FSMState> stateStack = new Stack<FSMState>();

    public void Update()
    {
        if (stateStack.Peek() != null)
            stateStack.Peek().Update(this);
    }

    public void PushState(FSMState state)
    {
        stateStack.Push(state);
        state.Enter();
    }

    public void PopState()
    {
        if (stateStack.Peek() != null)
        {
            stateStack.Peek().Exit();
            stateStack.Pop();
        }
    }
}