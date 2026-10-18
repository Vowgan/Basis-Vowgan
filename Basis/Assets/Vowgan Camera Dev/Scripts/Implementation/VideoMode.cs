using System;
using UnityEngine;

public class VideoMode : CameraMode
{

    public override string title => "Video";
    public override Sprite icon => null;
    public override int order => 0;

    private CameraModeEntry _timerEntry = new("Start Timer", null, 1, OnTimerChanged);
    private CameraModeEntry _resolutionEntry = new("Video Resolution", null, 0, OnResolutionChanged);


    [RuntimeInitializeOnLoadMethod]
    public static void AddToCamera() => HandheldCamera.AddCameraMode(new VideoMode());


    public override CameraModeEntry[] GetEntries() => new[]
    {
        _timerEntry,
        _resolutionEntry,
    };

    public override Action onPrimaryButtonPressed => () =>
    {
        Debug.Log("Video Mode action pressed.");
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
