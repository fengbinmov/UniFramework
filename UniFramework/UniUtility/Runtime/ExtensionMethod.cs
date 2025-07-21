using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethod
{
    public static T GetOrAddComp<T>(this Transform transform) where T : Component
    {
        return transform.gameObject.GetOrAddComp<T>();
    }
    public static T GetOrAddComp<T>(this GameObject gameObject) where T : Component
    {
        if (gameObject.TryGetComponent<T>(out var comp))
        {
            return comp;
        }
        else {
            return gameObject.AddComponent<T>();
        }
    }
}
