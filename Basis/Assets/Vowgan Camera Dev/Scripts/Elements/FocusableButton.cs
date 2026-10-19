using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Vowgan.HandheldCameraSystem
{
    [RequireComponent(typeof(Button))]
    public class FocusableButton : ButtonDescriptor
    {

        [SerializeField] protected bool _focused;

        [Header("Values")]
        [SerializeField] protected Vector2 _unfocusedSize = new Vector2(100, 100);
        [SerializeField] protected float _unfocusedScale = 1;
        [SerializeField] protected Color _unfocusedColor = Color.gray9;
        [SerializeField] protected Color _unfocusedBackgroundColor = Color.gray2;

        [SerializeField] protected Vector2 _focusedSize = new Vector2(100, 100);
        [SerializeField] protected float _focusedScale = 1;
        [SerializeField] protected Color _focusedColor = Color.gray2;
        [SerializeField] protected Color _focusedBackgroundColor = Color.gray9;


        protected override void OnValidate()
        {
            base.OnValidate();

            EditorEventQueue.QueueEvent(() =>
            {
                if (!this) return;
                SetFocused(_focused);
            });
        }

        public void SetFocused(bool value)
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
}
