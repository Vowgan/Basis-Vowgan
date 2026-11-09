using Unity.Scripting.LifecycleManagement;

namespace Vowgan.HandheldCameraSystem
{
    /// <summary>
    /// DO NOT USE: Instead use CameraMode‹T›.
    /// </summary>
    public abstract partial class CameraMode : ISortable
    {
        public abstract string name { get; }
        public abstract int order { get; }

        public abstract string spriteName { get; }

        /// <summary>
        /// Features are any defining part of a Camera Mode, though are often T typed to a specific mode.
        /// This needs to be implemented in a static context per T type.
        /// </summary>
        public abstract CallbackList<CameraFeature> features { get; }

        /// <summary>
        /// The action taken when the primary action (The center UI button / clicked while grabbed) is performed.
        /// For example: Use this to save the current render as a photo.
        /// </summary>
        public abstract void PrimaryAction();

        /// <summary>
        /// The action taken when the primary action (The center UI button / clicked while grabbed) is performed.
        /// For example: Use this to save the current render as a photo.
        /// </summary>
        public abstract void PrimaryAction(HandheldCameraPickup contextItem);
    }

    /// <summary>
    /// The T typing should be self-referential. This allows for a static list of CameraFeatures.
    /// When adding to the camera, it is recommended to use this method:
    /// <example>
    /// [UsedImplicitly]
    /// [RuntimeInitializeOnLoadMethod]
    /// private static void AddModes() => PhotoMode.AddFeature(new ResolutionFeature());
    /// </example>
    /// </summary>
    public abstract partial class CameraMode<T> : CameraMode where T : CameraMode
    {

        public override CallbackList<CameraFeature> features => _features;

        [AutoStaticsCleanup] private static CallbackList<CameraFeature> _features = new();

        public static void AddFeature(CameraFeature feature)
        {
            _features.Add(feature);
            _features.Sort();
        }

        public static void RemoveFeature(CameraFeature feature)
        {
            _features.Remove(feature);
        }
    }
}
