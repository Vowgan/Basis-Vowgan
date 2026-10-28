
using System;

public abstract class CameraFeature : ISortable
{

    public abstract string name { get; }
    public abstract int order { get; }
    /// <summary>
    /// The order in which this feature is evaluated on the features stack.
    /// Lower priority goes earlier.
    /// </summary>
    public abstract int priority { get; }
    public void Use()
    {
        OnUse();
        onUsed?.Invoke();
    }
    public abstract void OnUse();
    public event Action onUsed;

    public abstract void Evaluate(ref HandheldCameraDataState state);

}

public abstract class CameraFeature<TValue> : CameraFeature
{
    public abstract CallbackValue<TValue> state { get; }
}
