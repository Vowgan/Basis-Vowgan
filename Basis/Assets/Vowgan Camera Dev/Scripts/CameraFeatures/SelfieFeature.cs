using JetBrains.Annotations;
using UnityEngine;

public class SelfieFeature : CameraFeature<bool>
{

    [UsedImplicitly]
    [RuntimeInitializeOnLoadMethod]
    private static void AddModes() => PhotoMode.AddFeature(new SelfieFeature());


    public override string name => "Selfie";
    public override int order => 1;
    public override ValueField<bool> valueField { get; } = new(false);


    public override void SetValue(bool value)
    {
        if (valueField.SetValue(value))
        {
            Debug.Log($"{name} value updated to {value}");
        }
    }
}
