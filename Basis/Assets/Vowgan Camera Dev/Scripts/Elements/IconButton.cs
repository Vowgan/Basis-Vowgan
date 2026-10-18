using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IconButton : MonoBehaviour
{

    public Button.ButtonClickedEvent onClick => _button.onClick;

    public Sprite icon
    {
        get => _icon;
        set
        {
            _icon = value;
            if (!_iconElement) return;

            _iconElement.gameObject.SetActive(_icon);
            _iconElement.sprite = _icon;
        }
    }

    public string label
    {
        get => _labelElement.text;
        set
        {
            _label = value;
            if (!_labelElement) return;

            bool isEmpty = string.IsNullOrEmpty(_label);
            _labelElement.gameObject.SetActive(!isEmpty);
            _labelElement.text = _label;
        }
    }

    [SerializeField] private Sprite _icon;
    [SerializeField] private string _label;

    [SerializeField] private Button _button;
    [SerializeField] private Image _iconElement;
    [SerializeField] private TMP_Text _labelElement;


    private void OnValidate()
    {
        icon = _icon;
        label = _label;
    }
}
