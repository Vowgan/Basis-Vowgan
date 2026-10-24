using Unity.Scripting.LifecycleManagement;

/// <summary>
/// Total structure for the Handheld Camera. All interaction should be dependency-injected here.
/// This should be fully global and uninstanced.
///<example>
///    [RuntimeInitializeOnLoadMethod]
///    private static void AddToCamera() => HandheldCameraData.AddMode(new PhotoMode());
/// </example>
/// </summary>
public static partial class HandheldCameraData
{

    [AutoStaticsCleanup] public static CallbackList<CameraMode> modes = new();

    public static void AddMode(CameraMode mode)
    {
        modes.Add(mode);
        modes.Sort();
    }

    public static void RemoveMode(CameraMode mode)
    {
        modes.Remove(mode);
    }

}
