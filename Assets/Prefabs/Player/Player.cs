using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class Player : SerializedMonoBehaviour
{
    public static Player Instance;
    [SerializeField] private Animator animator;
    [SerializeField] private Dictionary<EquipmentType, List<PlayerCloth>> clothDatas = new Dictionary<EquipmentType, List<PlayerCloth>>();
    [SerializeField] private bool OnEnableUpdateWear = true;
    [SerializeField] private EventSE smashSound;

    private void OnEnable()
    {
        Instance = this;
        if(OnEnableUpdateWear)
            UpdateWear();
    }

    public static void UpdateWear()
    {
        if (Instance == null)
            return;
        PlayData playData = DataManager.Instance.playData;
        foreach (EquipmentType equip in Enum.GetValues(typeof(EquipmentType)))
        {
            if(Instance.clothDatas.ContainsKey(equip))
            {
                ItemClass itemClass = playData.GetNowEquip(equip);
                List<PlayerCloth> list = Instance.clothDatas[equip];
                list.ForEach(x => { x.SetCloth(itemClass == null ? "NULL" : itemClass.itemDataName); });
            }
        }
    }

    public static void Animation(string ani)
    {
        if(Instance.animator != null)
            Instance.animator.SetTrigger(ani);
    }

    public static void AttackPlayer()
    {
        Animation("Attack");
        Instance.Invoke("SmashSound", 0.005f);
    }

    public void SmashSound()
    {
        Instance.smashSound.RunEvent();
    }
}
