using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    public List<string> WatchedCategories = new List<string>();
    public bool WatchSelected = true;
    static UnityEngine.GameObject selected = null;

    static Debugger _instance;
    public static Debugger Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<Debugger>();
                UnityEditor.Selection.selectionChanged += OnSelectionChange;
            }
            return _instance;
        }
    }

    static void OnSelectionChange()
    {
        if (selected == UnityEditor.Selection.activeGameObject) return;
        if (selected != null)
            new List<IDebuggerListener>(selected.GetComponents<IDebuggerListener>()).ForEach(x => x.BlurDebug());
        selected = UnityEditor.Selection.activeGameObject;
        if (selected != null)
            new List<IDebuggerListener>(selected.GetComponents<IDebuggerListener>()).ForEach(x => x.FocusDebug());
    }

    public void Do<T>(Action act, UnityEngine.Object context)
    {
        if (IsMatch(typeof(T).FullName, context))
            act();
    }

    public void Log<T>(string message, UnityEngine.Object context)
    {
        if (IsMatch(typeof(T).FullName, context))
            Debug.Log(typeof(T).FullName + ": " + message);
    }

    bool IsMatch(string category, UnityEngine.Object context)
    {
        return (!WatchSelected || selected == context)
        && (WatchedCategories.Count == 0 || WatchedCategories.Any(x => category.IndexOf(x) > -1));
    }
}