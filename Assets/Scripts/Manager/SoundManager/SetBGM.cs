using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBGM : MonoBehaviour
{
    [SerializeField] private string bgmKey;
    [SerializeField] private string bgmName;
    [SerializeField] private bool destroyThis;
    [SerializeField] private SoundEvent eventType;
    [SerializeField] private bool loop;

    private void OnEnable()
    {
        if (eventType != SoundEvent.OnEnable)
            return;
        RunEvent();
    }

    private void OnDisable()
    {
        if (eventType != SoundEvent.OnDisable)
            return;
        RunEvent();
    }

    private void Awake()
    {
        if (eventType != SoundEvent.Awake)
            return;
        RunEvent();
    }

    private void Start()
    {
        if (eventType != SoundEvent.Start)
            return;
        RunEvent();
    }

    public virtual void RunEvent()
    {
        SoundManager.SetBGM_isKey(bgmName, bgmKey);
        SoundManager.Setloop(bgmKey, loop);

        if (destroyThis)
        {
            Destroy(this);
        }
    }
}
