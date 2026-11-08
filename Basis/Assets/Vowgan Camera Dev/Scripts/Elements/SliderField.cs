using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Vowgan.HandheldCameraSystem
{
    public class SliderField : MonoBehaviour
    {

        [field: SerializeField] public TMP_Text label { get; private set; }
        [field: SerializeField] public Slider slider { get; private set; }
        [field: SerializeField] public TMP_InputField input { get; private set; }

        public CallbackValue<float> value { get; } = new(0);


        private void Awake()
        {
            value.onChanged += val =>
            {
                slider.SetValueWithoutNotify(val);
                input.SetTextWithoutNotify(val.ToString(CultureInfo.InvariantCulture));
            };

            input.onValueChanged.AddListener(val =>
            {
                if (float.TryParse(val, out float result))
                {
                    value.SetValue(result);
                }
            });

            slider.onValueChanged.AddListener(val => { value.SetValue(val); });
        }


        public SliderField SetRange(float min, float max)
        {
            slider.minValue = min;
            slider.maxValue = max;

            return this;
        }

        public SliderField SetLabel(string text)
        {
            label.text = text;

            return this;
        }

    }
}
