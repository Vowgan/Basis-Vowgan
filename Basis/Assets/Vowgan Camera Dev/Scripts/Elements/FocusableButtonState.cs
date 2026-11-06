using System;
using Unity.Scripting.LifecycleManagement;

[NoAutoStaticsCleanup]
[Serializable]
public struct FocusableButtonState
{

    public string label;
    public string spriteName;
    public bool isFocused;

    public static readonly FocusableButtonState Empty;

}

