using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSE_IsKey : EventSE
{
    [SerializeField] private string key;
    public override void RunEvent()
    {
        SoundManager.PlaySE_IsKey(seName,key);

        if (destroyThis)
        {
            Destroy(this);
        }
    }
}
