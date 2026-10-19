using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Vowgan.HandheldCameraSystem
{
    public class VideoMode : ICameraMode
    {

        public string title => "Video";
        public Sprite icon => null;
        public int order => 0;

        private CameraModeEntry _timerEntry = new("Start Timer", null, 1, OnTimerChanged);
        private CameraModeEntry _resolutionEntry = new("Video Resolution", null, 0, OnResolutionChanged);


        [UsedImplicitly]
        [RuntimeInitializeOnLoadMethod]
        public static void AddToCamera() => HandheldCamera.modeManagement.AddCameraMode(new VideoMode());


        public CameraModeEntry[] GetEntries() => new[]
        {
            _timerEntry,
            _resolutionEntry,
        };

        public Action onPrimaryButtonPressed => () => { Debug.Log("Video Mode action pressed."); };


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
