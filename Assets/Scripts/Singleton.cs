using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    
    // If {} after a variable instead of ; it creates a property
    public static T Instance
    {
        get
        {
            return instance;
        }

        /*
        set
        {
            instance = value;
        }
        */
    }

    public bool IsInitialized
    {
        get
        {
            return instance != null;
        }
    }

    protected virtual void Awake()
    {
        if (IsInitialized)
        {
            Debug.LogError("[Singleton] Tried to create a second instance of a Singleton Class.");
            GameObject.Destroy(this.gameObject);
            // References current instance in this script and finds the gameObject with this script
        }

        else
        {
            instance = (T) this;
            // Converts it to T
        }

        DontDestroyOnLoad(this.gameObject);
    }

    protected virtual void OnDestroy()
    {
        instance = null;
    }

}
