using System;

namespace Vowgan.HandheldCameraSystem
{
    public class PhotoTimerFeature : CameraModeFeature
    {
        public enum TimerLength
        {
            Timer0,
            Timer3,
            Timer5,
            Timer10,
        }

        public override InteractType interactType => InteractType.Cycle;

        public override void Interact()
        {

        }

        public PhotoTimerFeature()
        {
            Init();
        }

        public void Init()
        {
            HandheldCameraSettings.current.RegisterString("PhotoTimerLength", interactType.ToString());
            HandheldCameraSettings.AddLoadListener(OnSettingsLoaded);
        }

        private const string _lengthSettingsKey = "PhotoTimerLength";
        private TimerLength _length;

        private void OnSettingsLoaded(HandheldCameraSettings settings)
        {
            if (settings.TryGetString(_lengthSettingsKey, out string output))
            {
                if (Enum.TryParse(output, out TimerLength result))
                {
                    _length = result;
                }
            }
        }

    }
}
