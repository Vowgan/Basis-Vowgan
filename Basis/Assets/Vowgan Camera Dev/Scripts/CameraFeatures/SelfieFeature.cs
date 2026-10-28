using JetBrains.Annotations;
using UnityEngine;

public sealed class SelfieFeature : CameraFeature<bool>
{

    [UsedImplicitly]
    [RuntimeInitializeOnLoadMethod]
    private static void AddModes() => PhotoMode.AddFeature(new SelfieFeature());


    public override string name => "Selfie";
    public override int order => 1;
    public override int priority => 0;
    public override CallbackValue<bool> state { get; } = new(false);


    public override void OnUse()
    {
        state.SetValue(!state.value);
        Debug.Log($"{name} value updated to {state}");
    }

    public override void Evaluate(ref HandheldCameraDataState dataState)
    {
        if (state.value) dataState.cameraYaw += 180;
        dataState.mirrored = state.value;
    }
}
