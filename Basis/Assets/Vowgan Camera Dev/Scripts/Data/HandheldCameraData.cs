using System;
using System.Linq;
using Unity.Scripting.LifecycleManagement;

namespace Vowgan.HandheldCameraSystem
{
    /// <summary>
    /// Total structure for the Handheld Camera. All interaction should be dependency-injected here.
    /// This should be fully global and uninstanced.
    /// <example>
    /// [RuntimeInitializeOnLoadMethod]
    /// private static void AddToCamera() => HandheldCameraData.AddMode(new PhotoMode());
    /// </example>
    /// </summary>
    [AutoStaticsCleanup]
    public static partial class HandheldCameraData
    {

        #region Modes

        public static CallbackList<CameraMode> modes = new();
        private static IOrderedEnumerable<CameraFeature> _priorityModes;

        /// <summary>
        /// Do not use SetValue for changing the CurrentMode.
        /// You should use SetCurrentMode on this class instead.
        /// </summary>
        public static CallbackValue<CameraMode> currentMode { get; private set; } = new(null);

        public static CallbackValue<HandheldCameraDataState> cameraState { get; private set; } = new(new());


        public static void SetCurrentMode(CameraMode mode)
        {
            if (currentMode.value == mode) return;

            if (!currentMode.IsNull())
            {
                foreach (CameraFeature feature in currentMode.value.features)
                {
                    feature.onUsed -= EvaluateState;
                }
            }

            currentMode.SetValue(mode);

            if (!currentMode.IsNull())
            {
                foreach (CameraFeature feature in currentMode.value.features)
                {
                    feature.onUsed += EvaluateState;
                }
            }

            EvaluateState();
        }

        public static void AddMode(CameraMode mode)
        {
            modes.Add(mode);
            modes.Sort();
        }

        public static void RemoveMode(CameraMode mode)
        {
            modes.Remove(mode);
        }

        public static void EvaluateState()
        {
            HandheldCameraDataState state = new();

            if (!currentMode.IsNull())
            {
                _priorityModes = currentMode.value.features.OrderBy(feature => feature.priority);
                foreach (CameraFeature feature in _priorityModes)
                {
                    feature.Evaluate(ref state);
                }
            }

            cameraState.SetValue(state);
        }

        #endregion

    }
}
