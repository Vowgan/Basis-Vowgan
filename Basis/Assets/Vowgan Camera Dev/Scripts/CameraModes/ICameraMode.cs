using System;
using UnityEngine;

namespace Vowgan.HandheldCameraSystem
{
    public interface ICameraMode
    {

        /*
         * To automatically add a mode to the camera system, implement like this:
         *    [RuntimeInitializeOnLoadMethod]
         *    public static void AddToCamera() => HandheldCamera.modeManagement.AddCameraMode(new MyCameraMode());
         */


        public string title { get; }
        public Sprite icon { get; }
        public int order { get; }
        public CameraModeEntry[] GetEntries();
        public Action onPrimaryButtonPressed { get; }

    }
}
