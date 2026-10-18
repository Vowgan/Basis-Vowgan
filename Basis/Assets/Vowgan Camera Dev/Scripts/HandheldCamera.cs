using System;
using System.Collections.Generic;
using UnityEngine;

public class HandheldCamera : MonoBehaviour
{

    public static HandheldCamera instance;


    #region Camera Mode

    public static CameraMode selectedMode;

    public static event Action<CameraMode> onModeAdded;
    public static event Action<CameraMode> onModeRemoved;
    public static event Action<CameraMode> onModeSelected;

    private static HashSet<CameraMode> _cameraModes { get; } = new();


    public static void AddCameraMode(CameraMode mode)
    {
        _cameraModes.Add(mode);
        onModeAdded?.Invoke(mode);
    }

    public static void RemoveCameraMode(CameraMode mode)
    {
        _cameraModes.Remove(mode);
        onModeRemoved?.Invoke(mode);
    }

    public static void SelectMode(CameraMode mode)
    {
        selectedMode = mode;
        onModeSelected?.Invoke(mode);
    }

    #endregion


    #region Camera Instance

    private void Awake()
    {
        if (instance && instance != this) instance.Close();

        instance = this;

        foreach (CameraMode mode in _cameraModes)
        {
            onModeAdded?.Invoke(mode);
        }
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }

    public void Close()
    {
        Destroy(gameObject);
    }

    #endregion



    public static void RunPrimaryAction()
    {
        if (selectedMode) selectedMode.onPrimaryButtonPressed?.Invoke();
    }

}
