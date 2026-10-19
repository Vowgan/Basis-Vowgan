using System;
using System.Collections.Generic;

namespace Vowgan.HandheldCameraSystem
{
    public class CameraModeManagement
    {
        public ICameraMode selectedMode;
        public event Action<ICameraMode> onModeAdded;
        public event Action<ICameraMode> onModeRemoved;
        public event Action<ICameraMode> onModeSelected;

        private HashSet<ICameraMode> _cameraModes { get; } = new();


        public void AddCameraMode(ICameraMode mode)
        {
            _cameraModes.Add(mode);
            onModeAdded?.Invoke(mode);
        }

        public void RemoveCameraMode(ICameraMode mode)
        {
            _cameraModes.Remove(mode);
            onModeRemoved?.Invoke(mode);
        }



        public void SelectMode(ICameraMode mode)
        {
            selectedMode = mode;
            onModeSelected?.Invoke(mode);
        }

        public void RunAllModesAdded()
        {
            foreach (ICameraMode mode in _cameraModes)
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
