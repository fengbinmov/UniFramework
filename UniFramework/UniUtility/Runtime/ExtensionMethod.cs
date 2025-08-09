using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public static void ToSameScene(this GameObject gameObject,GameObject target)
    {
        if (gameObject.scene.handle == target.scene.handle) return;

        SceneManager.MoveGameObjectToScene(gameObject, target.scene);
    }
}
