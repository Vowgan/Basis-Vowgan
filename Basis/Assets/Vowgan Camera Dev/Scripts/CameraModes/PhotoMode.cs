
using JetBrains.Annotations;
using UnityEngine;

namespace Vowgan.HandheldCameraSystem
{
    public sealed class PhotoMode : CameraMode<PhotoMode>
    {
        [UsedImplicitly]
        [RuntimeInitializeOnLoadMethod]
        private static void AddToCamera() => HandheldCameraData.AddMode(new PhotoMode());

        public override string name => "Photo";
        public override int order => 0;
        public override string spriteName => "Mode/Photo";

        public override void PrimaryAction()
        {
            Debug.Log("Taking photo. A contextpickup must be provided.");
        }

        public override void PrimaryAction(HandheldCameraPickup contextItem)
        {
            Debug.Log("Taking photo.");
            contextItem.previewRender.SavePhoto(contextItem.camera, contextItem.transform);
        }
    }
}
