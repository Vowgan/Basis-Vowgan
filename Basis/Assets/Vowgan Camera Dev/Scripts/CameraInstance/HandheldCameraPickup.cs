using Basis.Scripts.BasisSdk.Interactions;
using UnityEngine;

namespace Vowgan.HandheldCameraSystem
{
    /// <summary>
    /// Physical pickup in the scene. This plugs HCUI into the HCD, and handles pickup features.
    /// This must be instantiated in-scene.
    /// </summary>
    [RequireComponent(typeof(BasisPickupInteractable))]
    public class HandheldCameraPickup : MonoBehaviour
    {

        [SerializeField] private Camera _camera;
        [SerializeField] private Material _rtMaterial;
        [SerializeField] private CameraRender _previewRender = new();
        [SerializeField] private HandheldCameraUI _ui;
        [SerializeField] private Vector3 _desktopHeadOffset = new Vector3(0, 0, 0.5f);

        private static readonly int _baseMap = Shader.PropertyToID("_BaseMap");

        private BasisPickupInteractable _pickup;


        private void Awake()
        {
            _pickup = GetComponent<BasisPickupInteractable>();
            _pickup.OnPickupUse.AddListener(val =>
            {
                if (val != BasisPickUpUseMode.OnPickUpUseDown) return;
                HandheldCameraData.currentMode.value?.PrimaryAction();
            });
        }

        private void OnEnable()
        {
            _ui.Init();
            _previewRender.Create();
            _rtMaterial.SetTexture(_baseMap, _previewRender.rt);

            _previewRender.StartRender(_camera, this);

            HandheldCameraData.cameraState.onChanged += OnCameraStateChanged;
        }

        private void OnDisable()
        {
            _ui.Dispose();
            _previewRender.Dispose();
            HandheldCameraData.cameraState.onChanged -= OnCameraStateChanged;
        }

        private void OnCameraStateChanged(HandheldCameraDataState state)
        {
            _camera.transform.localEulerAngles = state.cameraEuler;
            _previewRender.isMirrored = state.mirrored;
        }
    }
}
