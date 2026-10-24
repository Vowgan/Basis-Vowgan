using UnityEngine;

public sealed class VideoMode : CameraMode<VideoMode>
{

    [RuntimeInitializeOnLoadMethod]
    private static void AddToCamera() => HandheldCameraData.AddMode(new VideoMode());

    public override string name => "Video";
    public override int order => 0;
}
