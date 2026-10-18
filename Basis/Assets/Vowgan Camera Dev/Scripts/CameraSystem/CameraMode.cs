using System;
using UnityEngine;

public abstract class CameraMode
{

    public abstract string title { get; }
    public abstract Sprite icon { get; }
    public abstract int order { get; }
    public abstract CameraModeEntry[] GetEntries();
    public abstract Action onPrimaryButtonPressed { get; }


    public static implicit operator bool(CameraMode mode) => mode != null;


}
