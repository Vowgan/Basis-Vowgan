
public abstract class CameraFeature : ISortable
{

    public abstract string name { get; }
    public abstract int order { get; }

}

public abstract class CameraFeature<TValue> : CameraFeature
{
    public abstract ValueChangedField<TValue> valueField { get; }
    public abstract void SetValue(TValue value);
}
