using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;


public class DecomposeItemSortUI : SerializedMonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject body;
    [SerializeField] private Dictionary<SortBy, GameObject> selectList = new Dictionary<SortBy, GameObject>();
    private bool isOn = false;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 윈도우를 ON OFF
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
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

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 정렬기준으로 정렬하는 버튼
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SortBtn(string sortBy)
    {
        SortBy sort = (SortBy)Enum.Parse(typeof(SortBy), sortBy);
        foreach (KeyValuePair<SortBy, GameObject> pair in selectList)
        {
            //현재 선택한 정렬버튼에 이펙트 표현
            if(pair.Key == sort)
                pair.Value.SetActive(true);
            else
                pair.Value.SetActive(false);
        }

        //정렬기준으로 정렬
        DecomposeUI.SortItemList(sort);
    }
}
