using JetBrains.Annotations;
using UnityEngine;

namespace Vowgan.HandheldCameraSystem
{
    public sealed class VideoMode : CameraMode<VideoMode>
    {
        [UsedImplicitly]
        [RuntimeInitializeOnLoadMethod]
        private static void AddToCamera() => HandheldCameraData.AddMode(new VideoMode());

        public override string name => "Video";
        public override int order => 0;
        public override string spriteName => "Mode/Video";


        public override void PrimaryAction()
        {
            Debug.Log("Starting video");
        }
    }
}
