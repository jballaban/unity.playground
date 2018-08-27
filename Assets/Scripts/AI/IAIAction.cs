using System.Collections.Generic;

public interface IAIAction
{
    void Reset();
    Dictionary<string, object> preConditions { get; }
    Dictionary<string, object> effects { get; }
    float cost { get; }
}