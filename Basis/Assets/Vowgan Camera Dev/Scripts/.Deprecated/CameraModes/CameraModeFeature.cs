namespace Vowgan.HandheldCameraSystem
{
    public abstract class CameraModeFeature
    {

        public enum InteractType
        {
            None,
            Press,
            Toggle,
            Cycle,
        }

        public abstract InteractType interactType { get; }
        public abstract void Interact();

    }
}
