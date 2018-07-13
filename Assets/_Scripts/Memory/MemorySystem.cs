using UnityEngine.Events;

public class MemorySystem : SubSystem
{
    public class RememberEvent : UnityEvent<int, object> { }
    public class ForgetEvent : UnityEvent<int, object> { }
}