using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Vowgan.HandheldCameraSystem
{
    public class PhotoMode : ICameraMode
    {

        public string title => "Photo";
        public Sprite icon => null;
        public int order => 0;

        private CameraModeEntry _timerEntry = new("Timer", null, 0, OnTimerChanged);
        private CameraModeEntry _resolutionEntry = new("Resolution", null, 1, OnResolutionChanged);


        [UsedImplicitly]
        [RuntimeInitializeOnLoadMethod]
        public static void AddToCamera() => HandheldCamera.modeManagement.AddCameraMode(new PhotoMode());


        public CameraModeEntry[] GetEntries() => new[]
        {
            _timerEntry,
            _resolutionEntry,
        };

        public Action onPrimaryButtonPressed => () => { Debug.Log("Photo Mode action pressed."); };


        private static void OnTimerChanged()
        {
            Debug.Log("Timer Changed");
        }

        private static void OnResolutionChanged()
        {
            Debug.Log("Resolution Changed");
        }

    }
}
