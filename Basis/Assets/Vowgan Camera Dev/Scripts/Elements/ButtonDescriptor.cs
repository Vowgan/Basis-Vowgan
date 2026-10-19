using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Vowgan.HandheldCameraSystem
{
    [RequireComponent(typeof(Button))]
    public class ButtonDescriptor : MonoBehaviour
    {

        public Button.ButtonClickedEvent onClick => button.onClick;

        public Button button
        {
            get
            {
                if (!_button) _button = GetComponent<Button>();
                return _button;
            }
        }


        [SerializeField] protected Sprite _icon;
        [SerializeField] protected string _label;

        [Header("References")]
        [SerializeField] protected Image _iconElement;
        [SerializeField] protected TMP_Text _labelElement;
        [SerializeField] protected Image _background;

        protected Button _button;


        protected virtual void OnValidate()
        {
            EditorEventQueue.QueueEvent(() =>
            {
                if (!this) return;

                SetIcon(_icon);
                SetLabel(_label);
            });
        }

        public void SetIcon(Sprite icon)
        {
            _icon = icon;
            if (_iconElement)
            {
                _iconElement.gameObject.SetActive(_icon);
                _iconElement.sprite = _icon;
            }
        }

        public void SetLabel(string label)
        {
            _label = label;
            if (_labelElement)
            {
                _labelElement.gameObject.SetActive(!string.IsNullOrEmpty(_label));
                _labelElement.SetText(_label);
            }
        }
    }
}
