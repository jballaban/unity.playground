using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class State : Dictionary<string, object>
{
    public State() : base() { }
    public State(State state) : base(state) { }
    public bool Test(Dictionary<string, object> state)
    {
        foreach (var kvp in state)
        {
            if (kvp.Value is bool && !(bool)kvp.Value) // check for defaults, non-exist is okay in this case
            {
                if (this.ContainsKey(kvp.Key) && (bool)this[kvp.Key])
                    return false;
            }
            else
            {
                if (!this.ContainsKey(kvp.Key) || !this[kvp.Key].Equals(kvp.Value))
                    return false;
            }
        }
        return true;
    }

    public void Apply(Dictionary<string, object> diff)
    {
        foreach (var key in diff.Keys)
            this[key] = diff[key];
    }
}