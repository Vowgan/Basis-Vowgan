using TMPro;
using UnityEngine;

namespace Vowgan.HandheldCameraSystem
{
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
        [SerializeField] private TimerButton _primaryActionButton;

        private CameraModeManagement _modeManagement;


        private void Awake()
        {
            _modeManagement = HandheldCamera.modeManagement;

            _modeManagement.onModeAdded += BuildMode;
            _modeManagement.onModeSelected += SetCurrentMode;

            _primaryActionButton.onClick.AddListener(_modeManagement.RunPrimaryAction);

            DestroyChildren(_modeContainer);
            DestroyChildren(_modeEntryContainer);
        }

        public void BuildMode(CameraMode mode)
        {
            FocusableButton modeButton = Instantiate(_modeButton, _modeContainer);
            modeButton.SetIcon(mode.icon);
            modeButton.SetLabel(mode.name);
            modeButton.SetFocused(false);

            modeButton.onClick.AddListener(() => _modeManagement.SelectMode(mode));
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

            foreach (CameraModeFeatureExperimental entry in mode.GetEntries())
            {
                FocusableButton entryButton = Instantiate(_modeEntryButton, _modeEntryContainer);
                entryButton.SetIcon(entry.icon);
                entryButton.SetLabel(entry.title);
                entryButton.SetFocused(entry.selected);
            }
        }
    }
}
