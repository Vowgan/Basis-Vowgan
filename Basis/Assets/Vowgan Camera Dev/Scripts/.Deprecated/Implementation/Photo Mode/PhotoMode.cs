using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Vowgan.HandheldCameraSystem
{
    public class PhotoMode : CameraMode<PhotoMode>
    {

        public override string name => "Photo";
        public override Sprite icon => null;
        public override int order => 0;

        [UsedImplicitly]
        [RuntimeInitializeOnLoadMethod]
        public static void AddToCamera() => HandheldCamera.modeManagement.AddCameraMode(new PhotoMode());


        public CameraModeFeatureExperimental[] GetEntries() => new[]
        {
            _timerFeature,
            _resolutionFeature,
        };

        public override Action onPrimaryButtonPressed => () => { Debug.Log("Photo Mode action pressed."); };


    }
}
