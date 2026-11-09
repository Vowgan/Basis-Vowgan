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

        [field: SerializeField] public CameraRender previewRender { get; private set; } = new();
        [field: SerializeField] public Camera camera { get; private set; }

        [SerializeField] private Material _rtMaterial;
        [SerializeField] private HandheldCameraUI _ui;

        private static readonly int _baseMap = Shader.PropertyToID("_BaseMap");

        private BasisPickupInteractable _pickup;


        private void Awake()
        {
            _pickup = GetComponent<BasisPickupInteractable>();
            _pickup.OnPickupUse.AddListener(val =>
            {
                if (val != BasisPickUpUseMode.OnPickUpUseDown) return;
                HandheldCameraData.currentMode.value?.PrimaryAction(this);
            });
        }

        private void OnEnable()
        {
            _ui.Init(this);
            previewRender.Create();
            _rtMaterial.SetTexture(_baseMap, previewRender.rt);

            previewRender.StartRender(camera, this);

            HandheldCameraData.cameraState.onChanged += OnCameraStateChanged;
        }

        private void OnDisable()
        {
            _ui.Dispose();
            previewRender.Dispose();
            HandheldCameraData.cameraState.onChanged -= OnCameraStateChanged;
        }

        private void OnCameraStateChanged(HandheldCameraDataState state)
        {
            camera.transform.localEulerAngles = state.cameraEuler;
            previewRender.isMirrored = state.mirrored;
        }
    }
}
