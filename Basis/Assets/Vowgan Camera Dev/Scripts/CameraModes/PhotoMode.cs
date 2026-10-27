
using JetBrains.Annotations;
using UnityEngine;

public sealed class PhotoMode : CameraMode<PhotoMode>
{
    [UsedImplicitly]
    [RuntimeInitializeOnLoadMethod]
    private static void AddToCamera() => HandheldCameraData.AddMode(new PhotoMode());

    public override string name => "Photo";
    public override int order => 0;
}
