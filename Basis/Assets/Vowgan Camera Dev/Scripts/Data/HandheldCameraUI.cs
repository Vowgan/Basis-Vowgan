using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vowgan.HandheldCameraSystem;

/// <summary>
/// Visual display for the HCD. This should be populated by a class such as HCP.
/// This should be instanced alongside the physical setup in-scene.
/// </summary>
[Serializable]
public class HandheldCameraUI : IDisposable
{

    public TimerButton primaryButton;
    public TMP_Text onScreenLog;
    public RectTransform modeOptionContainer;
    public FocusableButton modeOptionButton;
    public RectTransform modeFeatureContainer;
    public FocusableButton modeFeatureButton;
    public RectTransform hudOptionsContainer;
    public FocusableButton hudOptionButton;
    public RectTransform hudFeatureContainer;
    public FocusableButton hudFeatureButton;

    private Dictionary<CameraMode, FocusableButton> _modeOptionButtons = new();
    private Dictionary<CameraFeature, FocusableButton> _modeFeatureButtons = new();


    public void Init()
    {
        modeOptionContainer.DestroyChildren();
        modeFeatureContainer.DestroyChildren();
        hudOptionsContainer.DestroyChildren();
        hudFeatureContainer.DestroyChildren();

        HandheldCameraData.modes.onIndexAdded += OnModeAdded;
        HandheldCameraData.modes.onIndexRemoved += OnModeRemoved;
        HandheldCameraData.currentMode.onChanged += OnModeChanged;

        for (int i = 0; i < HandheldCameraData.modes.Count; i++)
        {
            OnModeAdded(i, HandheldCameraData.modes[i]);
        }
    }

    public void Dispose()
    {
        HandheldCameraData.modes.onIndexAdded -= OnModeAdded;
        HandheldCameraData.modes.onIndexRemoved -= OnModeRemoved;
        HandheldCameraData.currentMode.onChanged -= OnModeChanged;

        foreach (KeyValuePair<CameraMode, FocusableButton> modeButton in _modeOptionButtons)
        {
            UnityEngine.Object.Destroy(modeButton.Value.gameObject);
        }

        _modeOptionButtons.Clear();
    }

    private void OnModeAdded(int index, CameraMode mode)
    {
        if (_modeOptionButtons.TryGetValue(mode, out FocusableButton button))
        {
            UnityEngine.Object.Destroy(button.gameObject);
        }

        button = UnityEngine.Object.Instantiate(modeOptionButton, modeOptionContainer);
        button.SetLabel(mode.name);
        button.onClick.AddListener(() => HandheldCameraData.SetCurrentMode(mode));

        _modeOptionButtons[mode] = button;
    }

    private void OnModeRemoved(int index, CameraMode mode)
    {
        if (_modeOptionButtons.TryGetValue(mode, out FocusableButton button))
        {
            UnityEngine.Object.Destroy(button.gameObject);
            _modeOptionButtons.Remove(mode);
        }
    }

    private void OnModeChanged(CameraMode mode)
    {
        foreach (FocusableButton button in _modeFeatureButtons.Values)
        {
            UnityEngine.Object.Destroy(button.gameObject);
        }

        _modeFeatureButtons.Clear();

        foreach (CameraFeature feature in mode.features)
        {
            FocusableButton button = UnityEngine.Object.Instantiate(modeFeatureButton, modeFeatureContainer);
            button.SetLabel(feature.name);
            button.onClick.AddListener(feature.Use);

            _modeFeatureButtons[feature] = button;
        }

    }
}
