using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControllerManager : MonoBehaviour {

    private static bool exists;

    void Awake()
    {
        if (!exists)
        {
            exists = true;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Object.Destroy(this.gameObject);
        }
    }
}
