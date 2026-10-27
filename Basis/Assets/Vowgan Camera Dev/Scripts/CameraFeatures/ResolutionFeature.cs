
using JetBrains.Annotations;
using UnityEngine;

public enum ImageResolution
{
    x720,
    x1080,
    x1440,
    x2160,
}

public sealed class ResolutionFeature : CameraFeature<ImageResolution>
{

    [UsedImplicitly]
    [RuntimeInitializeOnLoadMethod]
    private static void AddModes() => PhotoMode.AddFeature(new ResolutionFeature());



    public override string name => "Resolution";
    public override int order => 0;
    public override ValueField<ImageResolution> valueField { get; } = new(ImageResolution.x720);

    public override void SetValue(ImageResolution newValue)
    {
        if (valueField.SetValue(newValue))
        {
            Debug.Log($"{name} value updated to {newValue}");
        }
    }
}
