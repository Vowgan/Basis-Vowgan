using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace Vowgan.HandheldCameraSystem
{
    public partial class HandheldCamera : MonoBehaviour
    {

        [AutoStaticsCleanup] public static HandheldCamera instance { get; private set; }
        [AutoStaticsCleanup] public static CameraModeManagement modeManagement { get; } = new();


        [SerializeField] private Camera _camera;


        private void Awake()
        {
            if (instance != this) instance?.Close();

            instance = this;

            modeManagement.RunAllModesAdded();

            HandheldCameraSettings.AddLoadListener(OnCameraSettingsLoaded);
        }

        private void OnCameraSettingsLoaded(HandheldCameraSettings settings)
        {
            settings.RegisterBool("MyBool", true);
            settings.SetNumber("MyNumber", 42);
        }

        private void OnDestroy()
        {
            if (instance == this) instance = null;
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}
