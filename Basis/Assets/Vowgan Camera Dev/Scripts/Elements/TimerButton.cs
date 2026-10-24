using System;
using Basis.BTween;
using UnityEngine;
using UnityEngine.UI;

namespace Vowgan.HandheldCameraSystem
{
    public class TimerButton : ButtonDescriptor
    {

        public bool useTimer;

        [SerializeField] protected Image _timerImage;

        public bool timerActive { get; private set; }
        private int _displayedTimer;

        
        public void RunTimer(float duration, Action onComplete)
        {
            if (timerActive)
            {
                Debug.LogWarning("Unable to start timer: Timer is already running.", this);
                return;
            }

            timerActive = true;
            _timerImage.enabled = true;
            _button.interactable = false;

            TweenImageFill tween = _timerImage.TweenFill(duration, 1, 0);
            tween.OnComplete += () =>
            {
                timerActive = false;
                _timerImage.enabled = false;
                _button.interactable = true;
            };
            tween.OnComplete += onComplete;
            tween.OnProcess += percent =>
            {
                percent = Mathf.Max(percent, float.Epsilon);
                int timeRemaining = Mathf.CeilToInt(duration / percent);
                DisplayTimer(timeRemaining);
            };
        }

        private void DisplayTimer(int remaining)
        {
            if (_displayedTimer == remaining) return;
            _displayedTimer = remaining;
            _labelElement.SetText(remaining.ToString());
        }

    }
}
