using System;
using UnityEngine;

public class PhotoMode : CameraMode
{

    public override string title => "Photo";
    public override Sprite icon => null;
    public override int order => 0;

    private CameraModeEntry _timerEntry = new("Timer", null, 0, OnTimerChanged);
    private CameraModeEntry _resolutionEntry = new("Resolution", null, 1, OnResolutionChanged);


    [RuntimeInitializeOnLoadMethod]
    public static void AddToCamera() => HandheldCamera.AddCameraMode(new PhotoMode());


    public override CameraModeEntry[] GetEntries() => new[]
    {
        _timerEntry,
        _resolutionEntry,
    };

    public override Action onPrimaryButtonPressed => () =>
    {
        Debug.Log("Photo Mode action pressed.");
    };


    private static void OnTimerChanged()
    {
        Debug.Log("Timer Changed");
    }

    private static void OnResolutionChanged()
    {
        Debug.Log("Resolution Changed");
    }

}
