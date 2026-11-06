
using JetBrains.Annotations;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;

public enum ImageResolution
{
    x720,
    x1080,
    x1440,
    x2160,
}

[AutoStaticsCleanup]
public sealed partial class ResolutionFeature : CameraFeature<ImageResolution>
{

    [UsedImplicitly]
    [RuntimeInitializeOnLoadMethod]
    private static void AddModes() => PhotoMode.AddFeature(new ResolutionFeature());



    public override string name => "Resolution";
    public override int order => 0;
    public override int priority => 0;
    public override CallbackValue<FocusableButtonState> featureState { get; } = new(_state1080);
    public override CallbackValue<ImageResolution> state { get; } = new(ImageResolution.x720);



    private const string ICON_PATH = "Feature/Resolution";

    [NoAutoStaticsCleanup] private static readonly FocusableButtonState _state720 = new()
    {
        label = "720",
        spriteName = ICON_PATH,
        isFocused = false
    };


    [NoAutoStaticsCleanup] private static readonly FocusableButtonState _state1080 = new()
    {
        label = "1080",
        spriteName = ICON_PATH,
        isFocused = false
    };


    [NoAutoStaticsCleanup] private static readonly FocusableButtonState _state1440 = new()
    {
        label = "1440",
        spriteName = ICON_PATH,
        isFocused = false
    };


    [NoAutoStaticsCleanup] private static readonly FocusableButtonState _state2160 = new()
    {
        label = "4K",
        spriteName = ICON_PATH,
        isFocused = false
    };


    public override void OnUse()
    {
        Debug.Log("TODO: Add resolution changes here.");
    }

    public override void Evaluate(ref HandheldCameraDataState dataState)
    {

    }
}
