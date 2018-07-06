using System.Collections.Generic;
using System.Linq;

public class State : Dictionary<string, object>
{
    public State() : base() { }
    public State(State state) : base(state) { }
    public bool Test(Dictionary<string, object> s)
    {
        return s.All(x => this.Contains(x));
    }

    public void Apply(Dictionary<string, object> diff)
    {
        foreach (var key in diff.Keys)
            this[key] = diff[key];
    }
}