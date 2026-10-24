using UnityEngine;

/// <summary>
/// Physical pickup in the scene. This plugs HCUI into the HCD, and handles pickup features.
/// This must be instantiated in-scene.
/// </summary>
public class HandheldCameraPickup : MonoBehaviour
{

    [SerializeField] private HandheldCameraUI ui;

    private void OnEnable()
    {
        ui.Init();
    }

    private void OnDisable()
    {
        ui.Dispose();
    }

}
