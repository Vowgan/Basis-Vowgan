using UnityEngine;

/// <summary>
/// Physical pickup in the scene. This plugs HCUI into the HCD, and handles pickup features.
/// This must be instantiated in-scene.
/// </summary>
public class HandheldCameraPickup : MonoBehaviour
{

    [SerializeField] private Camera _camera;
    [SerializeField] private Material _rtMaterial;
    [SerializeField] private CameraRender _previewRender = new();
    [SerializeField] private HandheldCameraUI _ui;

    private static readonly int _baseMap = Shader.PropertyToID("_BaseMap");


    private void OnEnable()
    {
        _ui.Init();
        _previewRender.Create();
        _rtMaterial.SetTexture(_baseMap, _previewRender.rt);

        _previewRender.StartRender(_camera, this);

        HandheldCameraData.cameraState.onChanged += OnCameraStateChanged;
    }

    private void OnCameraStateChanged(HandheldCameraDataState state)
    {
        _camera.transform.localEulerAngles = state.cameraEuler;
        _previewRender.mirrored = state.mirrored;
    }

    private void OnDisable()
    {
        _ui.Dispose();
        _previewRender.Dispose();
    }
}
