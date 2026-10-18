using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

/// <summary>
/// Safely queue an event to happen at the next available editor step.
/// This is the runtime-safe wrapper for EditorApplication.delayCall
/// </summary>
public static class EditorEventQueue
{
    public static void QueueEvent(Action action)
    {
#if UNITY_EDITOR
        EditorApplication.delayCall += () =>
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        };
#endif
    }
}

