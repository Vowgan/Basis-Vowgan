using System;
using UnityEngine;

[Serializable]
public struct HandheldCameraDataState
{

    public void Reset()
    {
        cameraEuler = Vector3.zero;
        mirrored = false;
    }


    public bool mirrored;

    #region Rotation

    public Vector3 cameraEuler { get; private set; }

    public float cameraPitch
    {
        get => cameraEuler.x;
        set
        {
            Vector3 euler = cameraEuler;
            euler.x = value;
            cameraEuler = euler;
        }
    }

    public float cameraYaw
    {
        get => cameraEuler.y;
        set
        {
            Vector3 euler = cameraEuler;
            euler.y = value;
            cameraEuler = euler;
        }
    }

    public float cameraRoll
    {
        get => cameraEuler.z;
        set
        {
            Vector3 euler = cameraEuler;
            euler.z = value;
            cameraEuler = euler;
        }
    }

    #endregion
}
