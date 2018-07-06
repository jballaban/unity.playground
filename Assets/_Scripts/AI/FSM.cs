using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    Stack<FSMState> stateStack = new Stack<FSMState>();

    public void Update(GameObject gameObject)
    {
        if (stateStack.Peek() != null)
            stateStack.Peek().Update(this, gameObject);

    }

    public void PushState(FSMState state)
    {
        stateStack.Push(state);
    }

    public void PopState()
    {
        stateStack.Pop();
    }
}