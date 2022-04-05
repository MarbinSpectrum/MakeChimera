using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class Manager : SerializedMonoBehaviour
{
    [System.NonSerialized] public bool loadComplete = false;
    public virtual void CallManager()
    {
        Debug.Log("Load");
    }
}
