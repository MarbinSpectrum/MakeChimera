using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class UI_base : SerializedMonoBehaviour
{
    [System.NonSerialized] public bool loadComplete = false;
    public virtual void CallUI()
    {
        Debug.Log("Load");
    }
}
