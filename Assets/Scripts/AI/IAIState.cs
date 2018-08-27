using System;
using System.Collections.Generic;

public interface IAIState : IDictionary<string, object>, ICloneable
{
    bool Test(IDictionary<string, object> preconditions);
    void Apply(IDictionary<string, object> effects);
}