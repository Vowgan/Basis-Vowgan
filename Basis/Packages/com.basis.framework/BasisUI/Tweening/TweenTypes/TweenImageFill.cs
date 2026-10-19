using UnityEngine.UI;

namespace Basis.BTween
{

    public static class TweenFillExtensions
    {
        public static TweenImageFill TweenFill(
            this Image target,
            float duration,
            float endValue)
        {
            TweenImageFill tween = TweenImageFill.GetAvailableTween()
                .SetTarget(target)
                .Start(duration, target.fillAmount, endValue);
            return tween;
        }

        public static TweenImageFill TweenFill(
            this Image target,
            float duration,
            float startValue,
            float endValue)
        {
            TweenImageFill tween = TweenImageFill.GetAvailableTween()
                .SetTarget(target)
                .Start(duration, startValue, endValue);
            return tween;
        }
    }

    public class TweenImageFill : BaseTweenFloat<TweenImageFill>
    {
        public Image Target;

        public TweenImageFill SetTarget(Image target)
        {
            Target = target;
            return this;
        }

        public override bool Process(double currentTime)
        {
            if (!Target)
            {
                Reset();
                return false;
            }

            if (base.Process(currentTime))
                return true;

            double t = BlendValue(currentTime);
            Target.fillAmount = (float)(StartValue + (EndValue - StartValue) * t);
            return false;
        }

        public override void Finish()
        {
            if (Target) Target.fillAmount = EndValue;
            base.Finish();
        }

        public override void Reset()
        {
            base.Reset();
            Target = null;
        }
    }
}
