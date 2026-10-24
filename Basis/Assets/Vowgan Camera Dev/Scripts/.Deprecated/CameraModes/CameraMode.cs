using System;
using System.Collections.Generic;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;


public abstract partial class CameraMode<T> : IComparable<ICameraFeature> where T: CameraMode<T>
{

    public abstract string name { get; }
    public abstract Sprite icon { get; }
    public abstract int order { get; }
    public abstract Action onPrimaryButtonPressed { get; }


    [AutoStaticsCleanup] public static List<CameraFeature<T>> features = new();


    public static void AddFeature(CameraFeature<T> feature)
    {
        features.Add(feature);
        features.Sort();
    }


    public int CompareTo(ICameraFeature other)
    {
        if (other == null) return 1;
        if (order < other.order) return -1;
        if (order > other.order) return 1;
        return string.CompareOrdinal(name, other.name);
    }
}
