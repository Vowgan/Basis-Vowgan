using Unity.Scripting.LifecycleManagement;

/// <summary>
/// When adding to the camera, it is recommended to use this method:
/// <example>
///    [RuntimeInitializeOnLoadMethod]
///    private static void AddToCamera() => PhotoMode.AddFeature(new TimerFeature());
/// </example>
/// </summary>
public abstract class CameraMode : ISortable
{
    public abstract string name { get; }
    public abstract int order { get; }
    public abstract CallbackList<CameraFeature> features { get; }
}

public abstract partial class CameraMode<T> : CameraMode where T : CameraMode
{

    public override CallbackList<CameraFeature> features => _features;

    [AutoStaticsCleanup] private static CallbackList<CameraFeature> _features = new();

    public static void AddFeature(CameraFeature feature)
    {
        _features.Add(feature);
        _features.Sort();
    }

    public static void RemoveFeature(CameraFeature feature)
    {
        _features.Remove(feature);
    }

}
