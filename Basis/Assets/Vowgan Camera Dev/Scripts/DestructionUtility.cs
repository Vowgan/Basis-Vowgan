using UnityEngine;

public static class DestructionUtility
{
    public static void DestroyChildren(this Transform transform)
    {
        if (!transform) return;
        int childCount = transform.childCount;
        if (childCount == 0) return;
        for (int i = childCount - 1; i >= 0 ; i--)
        {
            Object.Destroy(transform.GetChild(i).gameObject);
        }
    }
}
