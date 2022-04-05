using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopSE_IsKey : MonoBehaviour
{
    [SerializeField] public string key;
    [SerializeField] public bool destroyThis;
    [SerializeField] private SoundEvent eventType;
    public void OnEnable()
    {
        if (eventType != SoundEvent.OnEnable)
            return;
        StopEvent();
    }

    public void OnDisable()
    {
        if (eventType != SoundEvent.OnDisable)
            return;
        StopEvent();
    }

    public void Awake()
    {
        if (eventType != SoundEvent.Awake)
            return;
        StopEvent();
    }

    public void Start()
    {
        if (eventType != SoundEvent.Start)
            return;
        StopEvent();
    }

    public virtual void StopEvent()
    {
        SoundManager.StopSE_IsKey(key);

        if (destroyThis)
        {
            Destroy(this);
        }
    }
}
