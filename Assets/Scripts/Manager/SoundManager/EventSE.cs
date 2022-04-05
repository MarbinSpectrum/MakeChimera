using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSE : MonoBehaviour
{
    [SerializeField] public string seName;
    [SerializeField] public bool destroyThis;
    [SerializeField] private SoundEvent eventType;
    public void OnEnable()
    {
        if (eventType != SoundEvent.OnEnable)
            return;
        RunEvent();
    }

    public void OnDisable()
    {
        if (eventType != SoundEvent.OnDisable)
            return;
        RunEvent();
    }

    public void Awake()
    {
        if (eventType != SoundEvent.Awake)
            return;
        RunEvent();
    }

    public void Start()
    {
        if (eventType != SoundEvent.Start)
            return;
        RunEvent();
    }

    public virtual void RunEvent()
    {
        SoundManager.PlaySE(seName);

        if (destroyThis)
        {
            Destroy(this);
        }
    }
}
