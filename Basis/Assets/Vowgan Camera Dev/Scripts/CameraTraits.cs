using System;
using UnityEngine;

[Serializable]
public struct CameraTraits
{

    [Range(1e-05f, 179)] public float fieldOfView;
    public float nearClipPlane;
    public float farClipPlane;

    public CameraSensor.SensorType sensorType
    {
        get => CameraSensor.GetSensorType(sensorSize);
        set => sensorSize = CameraSensor.GetSizeInMillimeters(value);
    }

    public Vector2 sensorSize
    {
        get => _sensorSize;
        set
        {
            _sensorSize = value;
            sensorType = CameraSensor.GetSensorType(value);
        }
    }

    public int iso;
    public float shutterSpeed;
    public float focalLength;
    public Vector2 lensShift;
    public float aperture;
    public float focusDistance;

    private Vector2 _sensorSize;




    public void Apply(Camera camera)
    {
        if (!camera) return;

        camera.fieldOfView = fieldOfView;
        camera.nearClipPlane = nearClipPlane;
        camera.farClipPlane = farClipPlane;
        camera.usePhysicalProperties = true;
        camera.sensorSize = sensorSize;
        camera.iso = Mathf.Max(iso, 1);
        camera.shutterSpeed = shutterSpeed;
        camera.focalLength = focalLength;
        camera.lensShift = lensShift;
        camera.aperture = aperture;
        camera.focusDistance = focusDistance;
    }

    public static readonly CameraTraits Default = new()
    {
        fieldOfView = 90,
        nearClipPlane = 0.01f,
        farClipPlane = 1000f,
        sensorSize = new(36, 24),
        iso = 200,
        shutterSpeed = 0.005f,
        focalLength = 12,
        lensShift = Vector2.zero,
        aperture = 15.14f,
        focusDistance = 10,
    };

}
