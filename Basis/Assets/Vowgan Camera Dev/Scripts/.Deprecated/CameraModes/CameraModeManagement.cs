using System;
using System.Collections.Generic;

namespace Vowgan.HandheldCameraSystem
{
    public class CameraModeManagement
    {
        public CameraMode selectedMode;
        public event Action<CameraMode> onModeAdded;
        public event Action<CameraMode> onModeRemoved;
        public event Action<CameraMode> onModeSelected;

        private HashSet<CameraMode> _cameraModes { get; } = new();


        public void AddCameraMode(CameraMode mode)
        {
            _cameraModes.Add(mode);
            onModeAdded?.Invoke(mode);
        }

        public void RemoveCameraMode(CameraMode mode)
        {
            _cameraModes.Remove(mode);
            onModeRemoved?.Invoke(mode);
        }



        public void SelectMode(CameraMode mode)
        {
            selectedMode = mode;
            onModeSelected?.Invoke(mode);
        }

        public void RunAllModesAdded()
        {
            foreach (CameraMode mode in _cameraModes)
            {
                onModeAdded?.Invoke(mode);
            }
        }

        /// <summary>
        /// If a mode is currently selected and it has a primary action, run this action.
        /// </summary>
        public void RunPrimaryAction()
        {
            selectedMode?.onPrimaryButtonPressed?.Invoke();
        }

    }
}
