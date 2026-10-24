using System;
using UnityEngine;

namespace Vowgan.HandheldCameraSystem
{
    [Serializable]
    public class HandheldCameraSettings : JsonSettings<HandheldCameraSettings>
    {
        protected override string settingsPath => "VowganCameraSettings";

        [RuntimeInitializeOnLoadMethod]
        private static void PopulateImmediate()
        {
            current = new();
            current.LoadSettingsFromFile();
        }

        protected override void OnSettingsLoaded()
        {
            base.OnSettingsLoaded();
            Debug.Log("HHCS: Settings loaded.");
        }

        protected override void OnSettingsWritten()
        {
            base.OnSettingsWritten();
            Debug.Log("HHCS: Settings written.");
        }
    }
}
