using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class FocusableButton : MonoBehaviour
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


    [SerializeField] private Sprite _icon;
    [SerializeField] private string _label;
    [SerializeField] private bool _focused;

    [Header("Values")]
    [SerializeField] private Vector2 _unfocusedSize = new Vector2(100, 100);
    [SerializeField] private float _unfocusedScale = 1;
    [SerializeField] private Color _unfocusedColor = Color.gray9;
    [SerializeField] private Color _unfocusedBackgroundColor = Color.gray2;

    [SerializeField] private Vector2 _focusedSize = new Vector2(100, 100);
    [SerializeField] private float _focusedScale = 1;
    [SerializeField] private Color _focusedColor = Color.gray2;
    [SerializeField] private Color _focusedBackgroundColor = Color.gray9;

    [Header("References")]
    [SerializeField] private Button _button;
    [SerializeField] private Image _iconElement;
    [SerializeField] private TMP_Text _labelElement;
    [SerializeField] private Image _background;


    private void OnValidate()
    {
        EditorEventQueue.QueueEvent(() =>
        {
            if (!this) return;

            SetIcon(_icon);
            SetLabel(_label);
            SetSelected(_focused);
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

    public void SetSelected(bool value)
    {
        _focused = value;

        if (_iconElement)
        {
            _iconElement.color = _focused
                ? _focusedColor
                : _unfocusedColor;
        }

        if (_labelElement)
        {
            _labelElement.color = _focused
                ? _focusedColor
                : _unfocusedColor;
        }

        if (_background)
        {
            _background.color = _focused
                ? _focusedBackgroundColor
                : _unfocusedBackgroundColor;
        }

        transform.localScale = Vector3.one * (_focused ? _focusedScale : _unfocusedScale);

        RectTransform rt = (RectTransform)transform;
        if (rt) rt.sizeDelta = _focused ? _focusedSize : _unfocusedSize;
    }
}
