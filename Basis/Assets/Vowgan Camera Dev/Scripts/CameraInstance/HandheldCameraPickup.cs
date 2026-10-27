using UnityEngine;

/// <summary>
/// Physical pickup in the scene. This plugs HCUI into the HCD, and handles pickup features.
/// This must be instantiated in-scene.
/// </summary>
public class HandheldCameraPickup : MonoBehaviour
{

    [SerializeField] private HandheldCameraUI _ui;
    [SerializeField] private Camera _camera;
    [SerializeField] private CameraRender _previewRender = new();
    [SerializeField] private Material _rtMaterial;

    private static readonly int _baseMap = Shader.PropertyToID("_BaseMap");


    private void OnEnable()
    {
        _ui.Init();
        _previewRender.Create();
        _rtMaterial.SetTexture(_baseMap, _previewRender.rt);

        _previewRender.StartRender(_camera, this);
    }

    private void OnDisable()
    {
        _ui.Dispose();
        _previewRender.Dispose();
    }
}
