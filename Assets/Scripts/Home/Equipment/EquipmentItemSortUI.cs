using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class EquipmentItemSortUI : SerializedMonoBehaviour
{
    [SerializeField] private Dictionary<SortBy, GameObject> selectList = new Dictionary<SortBy, GameObject>();
    [SerializeField] private GameObject body;
    [SerializeField] private Animator animator;
    private bool isOn = false;
    public void OnOffWindow()
    {
        OnOffWindow(!isOn);
    }
    public void OnOffWindow(bool state)
    {
        isOn = state;
        if (isOn)
            animator.SetTrigger("ON");
        else
            animator.SetTrigger("OFF");
    }
    public void SortBtn(string sortBy)
    {
        foreach (KeyValuePair<SortBy, GameObject> pair in selectList)
            pair.Value.SetActive(false);
        SortBy sort = (SortBy)Enum.Parse(typeof(SortBy), sortBy);
        selectList[sort].SetActive(true);

        EquipmentUI.SortItemList(sort);
    }
}
