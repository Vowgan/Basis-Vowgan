using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class HandheldCameraUI : MonoBehaviour
{

    [SerializeField] private HandheldCamera _hhc;

    [Header("Prefabs")]
    [SerializeField] private FocusableButton _modeButton;
    [SerializeField] private FocusableButton _modeEntryButton;
    [SerializeField] private FocusableButton _hudModeButton;

    [Header("Components")]
    [SerializeField] private RectTransform _modeContainer;
    [SerializeField] private RectTransform _modeEntryContainer;
    [SerializeField] private RectTransform _onscreenUIContainer;
    [SerializeField] private RectTransform _onscreenUIContextContainer;
    [SerializeField] private TMP_Text _screenDisplayLabel;
    [SerializeField] private Button _primaryActionButton;



    private void Awake()
    {
        HandheldCamera.onModeAdded += BuildMode;
        HandheldCamera.onModeSelected += SetCurrentMode;

        _primaryActionButton.onClick.AddListener(HandheldCamera.RunPrimaryAction);

        DestroyChildren(_modeContainer);
        DestroyChildren(_modeEntryContainer);
    }

    public void BuildMode(CameraMode mode)
    {
        FocusableButton modeButton = Instantiate(_modeButton, _modeContainer);
        modeButton.SetIcon(mode.icon);
        modeButton.SetLabel(mode.title);
        modeButton.SetSelected(false);

        modeButton.onClick.AddListener(() => HandheldCamera.SelectMode(mode));
    }

    private void DestroyChildren(RectTransform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }

    private void SetCurrentMode(CameraMode mode)
    {
        DestroyChildren(_modeEntryContainer);

        foreach (CameraModeEntry entry in mode.GetEntries())
        {
            FocusableButton entryButton = Instantiate(_modeEntryButton, _modeEntryContainer);
            entryButton.SetIcon(entry.icon);
            entryButton.SetLabel(entry.title);
            entryButton.SetSelected(entry.selected);
        }
    }

}
