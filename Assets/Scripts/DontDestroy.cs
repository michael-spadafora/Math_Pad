using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    // from https://answers.unity.com/questions/1124640/how-to-make-the-warning-dontdestroyonload-only-wor.html
    public static void DontDestroyChildOnLoad(GameObject child)
    {
        Transform parentTransform = child.transform;

        // If this object doesn't have a parent then its the root transform.
        while (parentTransform.parent != null)
        {
            // Keep going up the chain.
            parentTransform = parentTransform.parent;
        }
        GameObject.DontDestroyOnLoad(parentTransform.gameObject);
    }

    private void Awake()
    {
        DontDestroyChildOnLoad(transform.gameObject);
    }
}
