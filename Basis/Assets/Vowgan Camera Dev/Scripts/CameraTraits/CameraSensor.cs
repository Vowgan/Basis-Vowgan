using System;
using UnityEngine;

public static class CameraSensor
{

// Most of these sizes start with numbers, so for consistency,
// they've all been given underscores on their names.

    public enum SensorType
    {
        [InspectorName("8mm")] _8mm,
        [InspectorName("Super 8mm")] _Super8mm,
        [InspectorName("16mm")] _16mm,
        [InspectorName("Super 16mm")] _Super16mm,
        [InspectorName("35mm 2-perf")] _35mm2perf,
        [InspectorName("35mm Academy")] _35mmAcademy,
        [InspectorName("Super-35")] _Super35,
        [InspectorName("35mm TV Projection")] _35mmTVProjection,
        [InspectorName("35mm Full Aperture")] _35mmFullAperture,
        [InspectorName("35mm 1.85 Projection")] _35mm185Projection,
        [InspectorName("35mm Anamorphic")] _35mmAnamorphic,
        [InspectorName("65mm ALEXA")] _65mmALEXA,
        [InspectorName("70mm")] _70mm,
        [InspectorName("70mm IMAX")] _70mmIMAX,
        [InspectorName("Custom")] _Custom,
    }

    private static readonly Vector2[] _sizes =
    {
        new(4.8f, 3.5f), // 8mm
        new(5.79f, 4.01f), // Super 8mm
        new(10.26f, 7.49f), // 16mm
        new(12.522f, 7.417f), // Super 16mm
        new(21.95f, 9.35f), // 35mm 2-perf
        new(21.946f, 16.002f), // 35mm Academy
        new(24.89f, 18.66f), // Super-35
        new(20.726f, 15.545f), // 35mm TV Projection
        new(24.892f, 18.669f), // 35mm Full Aperture
        new(20.955f, 11.328f), // 35mm 1.85 Projection
        new(21.946f, 18.593f), // 35mm Anamorphic
        new(54.12f, 25.59f), // 65mm ALEXA
        new(52.476f, 23.012f), // 70mm
        new(70.41f, 52.63f), // 70mm IMAX
        new(0f, 0f), // Custom
    };

    public static Vector2 GetSizeInMillimeters(SensorType type) => _sizes[(int)type];

    public static SensorType GetSensorType(Vector2 size)
    {
        int index = Array.IndexOf(_sizes, size);
        if (index == -1) return SensorType._Custom;
        return (SensorType)index;
    }

}
