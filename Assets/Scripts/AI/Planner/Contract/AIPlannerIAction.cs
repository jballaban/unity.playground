using System.Collections.Generic;

public interface AIPlannerIAction
{
    void Reset();
    Dictionary<string, object> preConditions { get; }
    Dictionary<string, object> effects { get; }
    float cost { get; }
}