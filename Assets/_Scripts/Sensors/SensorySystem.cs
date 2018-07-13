using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SensorySystem : SubSystem
{
    public class ObserveEnterEvent : UnityEvent<GameObject> { }

    public class ObserveExitEvent : UnityEvent<GameObject> { }

}