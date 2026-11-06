using JetBrains.Annotations;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;

[AutoStaticsCleanup]
public sealed partial class SelfieFeature : CameraFeature<bool>
{

    [UsedImplicitly]
    [RuntimeInitializeOnLoadMethod]
    private static void AddModes()
    {
        SelfieFeature feature = new();
        PhotoMode.AddFeature(feature);
        VideoMode.AddFeature(feature);
    }


    public override string name => "Selfie";
    public override int order => 1;
    public override int priority => 0;
    public override CallbackValue<FocusableButtonState> featureState { get; } = new(_stateDisabled);
    public override CallbackValue<bool> state { get; } = new(false);




    private const string ICON_PATH = "Feature/Selfie";

    [NoAutoStaticsCleanup] private static readonly FocusableButtonState _stateDisabled = new()
    {
        label = "Selfie",
        spriteName = ICON_PATH,
        isFocused = false
    };

    [NoAutoStaticsCleanup] private static readonly FocusableButtonState _stateEnabled = new()
    {
        label = "Selfie",
        spriteName = ICON_PATH,
        isFocused = true
    };



    public override void OnUse()
    {
        state.SetValue(!state.value);
        Debug.Log($"{name} value updated to {state}");
        featureState.SetValue(state.value ? _stateEnabled : _stateDisabled);
    }

    public override void Evaluate(ref HandheldCameraDataState dataState)
    {
        if (state.value) dataState.cameraYaw += 180;
        dataState.mirrored = state.value;
    }

}
